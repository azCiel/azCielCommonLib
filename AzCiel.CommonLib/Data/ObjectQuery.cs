/*
 * Copyright (c) 2009 HAKKO Development Co.,Ltd. az'Ciel division.
 * All Rights Reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/*
 * 簡易 O/R マッピング
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace AzCiel.CommonLib.Data {

    /// <summary>
    /// 簡易 O/R マッピングクラス
    /// </summary>
    public class ObjectQuery {

        // DB オブジェクトファクトリ
        private static DbProviderFactory factory__ = null;
        // DB コネクション
        private DbConnection conn_;

        public static DbProviderFactory DbFactory {
            set {
                factory__ = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="conn">コネクションオブジェクト</param>
        public ObjectQuery(DbConnection conn) {
            conn_ = conn;
            if (factory__ == null) {
                factory__ = DbProviderFactories.GetFactory(conn.GetType().Namespace);
            }
        }

        // プロパティがプライマリキーか？
        private static bool IsPrimarykey(PropertyInfo pi) {
            object[] attrs = pi.GetCustomAttributes(typeof(DbPrimaryKeyAttribute), true);
            return (attrs != null && attrs.Length > 0);
        }

        // プロパティに対応するカラム名取得
        private static string GetFiledName(PropertyInfo pi) {
            object[] attrs = pi.GetCustomAttributes(typeof(DbFieldAttribute), true);
            if (attrs != null && attrs.Length > 0) {
                return ((DbFieldAttribute)attrs[0]).FieldName;
            } else {
                return pi.Name;
            }
        }

        // クラスに対応するテーブル名取得
        private static string GetTableName(Type objType) {
            object[] attrs = objType.GetCustomAttributes(typeof(DbTableNameAttribute), false);
            if (attrs != null && attrs.Length > 0) {
                return ((DbTableNameAttribute)attrs[0]).TableName;
            } else {
                return objType.Name;
            }
        }

        // オブジェクトに対応するテーブル名取得
        private static string GetTableName(object obj) {
            return GetTableName(obj.GetType());
        }

        /// <summary>
        /// DataSet -> object 変換
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="ds">DataSet</param>
        /// <returns>オブジェクトのリスト</returns>
        public static IList<T> BuildResultSet<T>(DataSet ds) {
            IList<T> result = new List<T>();

            foreach (DataRow row in ds.Tables[0].Rows) {
                T item = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(null);
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++) {
                    DataColumn col = ds.Tables[0].Columns[i];
                    PropertyInfo[] pis = item.GetType().GetProperties();
                    foreach (PropertyInfo pi in pis) {
                        if (GetFiledName(pi) == col.ColumnName) {
                            if (Convert.IsDBNull(row.ItemArray[i])) {
                                pi.SetValue(item, null, null);
                            } else {
                                pi.SetValue(item, row.ItemArray[i], null);
                            }
                            break;
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 更新パラメータ (オブジェクト版) の生成
        /// </summary>
        /// <param name="param">更新パラメータ</param>
        public static IDictionary<string, object> GetParams(object param) {
            IDictionary<string, object> result = new Dictionary<string, object>();

            Type objType = param.GetType();
            PropertyInfo[] pis = objType.GetProperties();
            foreach (PropertyInfo pi in pis) {
                object[] attrs = pi.GetCustomAttributes(typeof(DbAutoGenerateFieldAttribute), true);
                if (attrs == null || attrs.Length == 0) {
                    result.Add(GetFiledName(pi), pi.GetValue(param, null));
                }
            }
            return result;
        }

        /// <summary>
        /// DB からオブジェクトを読み込む (SELECT)
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="where">WHERE句 (プレースホルダあり) / ORDER BY句</param>
        /// <param name="param">条件パラメータ</param>
        /// <returns>読み込んだオブジェクトリスト</returns>
        public IList<T> GetObjects<T>(string where, ICollection<object> param) {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT * FROM ");
            query.Append(GetTableName(typeof(T)));
            query.Append(@" ");
            query.Append(where);
            QueryBuilder qb = new QueryBuilder(query.ToString(), param);

            using (DbCommand cmd = qb.Result(conn_))
            using (DataSet ds = new DataSet())
            using (DbDataAdapter da = factory__.CreateDataAdapter()) {
                da.SelectCommand = cmd;
                da.Fill(ds);
                return BuildResultSet<T>(ds);
            }
        }

        /// <summary>
        /// DB からオブジェクトを読み込む (SELECT)
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="where">WHERE句 (プレースホルダなし) / ORDER BY句</param>
        /// <returns>読み込んだオブジェクトリスト</returns>
        public IList<T> GetObjects<T>(string where) {
            return GetObjects<T>(where, new object[0]);
        }

        /// <summary>
        /// DB からオブジェクトを読み込む (SELECT)
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="where">WHERE句 (プレースホルダあり)</param>
        /// <param name="param">条件パラメータ</param>
        /// <returns>読み込んだオブジェクトリスト</returns>
        public IList<T> GetObjects<T>(string where, object param) {
            return GetObjects<T>(where, new object[] { param });
        }

        /// <summary>
        /// DB からオブジェクトを読み込む (SELECT)
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <returns>読み込んだオブジェクトリスト</returns>
        public IList<T> GetObjects<T>() {
            return GetObjects<T>(null, null);
        }

        /// <summary>
        /// プライマリキー項目の WHERE 句を取得
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="obj">プライマリキー項目を設定したオブジェクト</param>
        /// <param name="param">パラメータ群返却先</param>
        /// <returns>WHERE句文字列</returns>
        public string GetPrimaryWhere<T>(T obj, out ICollection<object> param) {
            StringBuilder where = new StringBuilder();
            param = new List<object>();

            PropertyInfo[] pis = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pis) {
                if (IsPrimarykey(pi)) {
                    if (where.Length > 0) {
                        where.Append(@" AND ");
                    }
                    where.Append(GetFiledName(pi));
                    where.Append(@"=?");
                    param.Add(pi.GetValue(obj, null));
                }
            }
            return where.ToString();
        }

        /// <summary>
        /// オブジェクト内容の再読み込み
        /// </summary>
        /// <typeparam name="T">返却オブジェクト型</typeparam>
        /// <param name="obj">プライマリキー項目を設定したオブジェクト</param>
        /// <returns>再読み込みしたオブジェクト</returns>
        public T LoadObject<T>(T obj) {
            ICollection<object> param;
            string where = @"WHERE " + GetPrimaryWhere<T>(obj, out param);
            IList<T> list = GetObjects<T>(where, param);
            if (list.Count == 0) {
                return default(T);
            }
            return obj = list[0];
        }

        /// <summary>
        /// オブジェクト挿入 (INSERT)
        /// </summary>
        /// <typeparam name="T">オブジェクト型</typeparam>
        /// <param name="obj">挿入するオブジェクト</param>
        public void InsertObject<T>(T obj) {
            UpdateQueryBuilder qb = new UpdateQueryBuilder(GetTableName(obj));
            qb.SetParams(GetParams(obj));
            using (DbCommand cmd = qb.Result(conn_, UpdateType.INSERT)) {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// オブジェクト内容で更新 (UPDATE)
        /// </summary>
        /// <typeparam name="T">オブジェクト型</typeparam>
        /// <param name="obj">更新するオブジェクト</param>
        public void UpdateObject<T>(T obj) {
            T tmp = LoadObject(obj);
            if (tmp == null) {
                InsertObject(obj);
            } else {
                UpdateQueryBuilder qb = new UpdateQueryBuilder(GetTableName(obj));
                qb.SetParams(GetParams(obj));
                ICollection<object> param;
                qb.SetWhere(GetPrimaryWhere<T>(obj, out param), param);
                using (DbCommand cmd = qb.Result(conn_, UpdateType.UPDATE)) {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// コレクションオブジェクトの内容をすべて更新
        /// </summary>
        /// <typeparam name="T">オブジェクト型</typeparam>
        /// <param name="recs">オブジェクトのコレクション</param>
        public void SaveAll<T>(ICollection<T> recs) {
            foreach (T rec in recs) {
                UpdateObject(rec);
            }
        }

        /// <summary>
        /// オブジェクト内容を削除
        /// </summary>
        /// <typeparam name="T">オブジェクト型</typeparam>
        /// <param name="obj">削除対象オブジェクト</param>
        public void DeleteObject<T>(T obj) {
            ICollection<object> param;
            string where = GetPrimaryWhere<T>(obj, out param);

            StringBuilder query = new StringBuilder();
            query.Append(@"DELETE * FROM ");
            query.Append(GetTableName(obj));
            query.Append(@" WHERE ");
            query.Append(where);
            QueryBuilder qb = new QueryBuilder(query.ToString(), param);
            using (DbCommand cmd = qb.Result(conn_)) {
                cmd.ExecuteNonQuery();
            }
        }

    }
}
/*
 * -*- settings for emacs. -*-
 * Local Variables:
 * tab-width: 4
 * indent-tabs-mode: nil
 * c-basic-offset: 4
 */
