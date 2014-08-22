/*
 * Copyright (c) 2009 HAKKO Development Co.,Ltd. az'Ciel division.
 * Copyright (c) 2008 az'Ciel HAKKO Co.,Ltd.
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
 * 更新用 DbCommand ビルダークラス
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.Common;
using AzCiel.CommonLib.Text;

namespace AzCiel.CommonLib.Data {

    /// <summary>
    /// INSERT / UPDATE 判別用列挙子
    /// </summary>
    public enum UpdateType {
        /// <summary>
        /// INSERT クエリ
        /// </summary>
        INSERT,

        /// <summary>
        /// UPDATE クエリ
        /// </summary>
        UPDATE
    }

    /// <summary>
    /// 更新用 DbCommand ビルダークラス
    /// </summary>
    public class UpdateQueryBuilder {

        // テーブル名
        private string tblName_;
        // 更新パラメータ
        private IDictionary<string, object> params_;
        // WHERE 構文
        private string whereQuery_ = null;
        // WHERE パラメータ
        private ICollection<object> whereParams_ = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// param は、キーがカラム名で値が更新値
        /// </remarks>
        /// <param name="tblName">テーブル名</param>
        public UpdateQueryBuilder(string tblName) {
            tblName_ = tblName;
        }

        /// <summary>
        /// 更新パラメータの設定
        /// </summary>
        /// <param name="param">更新パラメータ</param>
        public void SetParams(IDictionary<string, object> param) {
            params_ = param;
        }

        /// <summary>
        /// 更新パラメータのクリア
        /// </summary>
        public void ClearParams() {
            params_.Clear();
        }

        /// <summary>
        /// UPDATE 用 WHERE 句の設定
        /// </summary>
        /// <param name="query">WHERE 句文字列</param>
        /// <param name="param">WHERE 用パラメータ列</param>
        public void SetWhere<T>(string query, ICollection<T> param) {
            whereQuery_ = query;
            whereParams_ = (ICollection<object>)param;
        }

        /// <summary>
        /// UPDATE 用 WHERE 句の設定
        /// </summary>
        /// <param name="query">WHERE 句文字列</param>
        /// <param name="param">WHERE 用パラメータ</param>
        public void SetWhere<T>(string query, T param) {
            whereQuery_ = query;
            if (whereParams_ != null) {
                whereParams_.Clear();
            } else {
                whereParams_ = new List<object>();
            }
            whereParams_.Add(param);
        }

        // INSERT 文生成
        private string createInsertQuery(IDictionary<string, object> param) {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"INSERT INTO ");
            sql.Append(tblName_);
            sql.Append(@" (");
            sql.Append(StrJoins.Join(@",", param.Keys));
            sql.Append(@") VALUES (");

            for (int i=0; i<param.Count; i++) {
                if (i > 0) {
                    sql.Append(@",");
                }
                sql.Append(@"?");
            }

            sql.Append(@")");

            return sql.ToString();
        }

        // UPDATE 文生成
        private string createUpdateQuery(IDictionary<string, object> param) {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"UPDATE ");
            sql.Append(tblName_);
            sql.Append(@" SET ");

            int i = 0;
            foreach (string key in param.Keys) {
                if (i > 0) {
                    sql.Append(@",");
                }
                sql.Append(key);
                sql.Append(@"=?");
                i++;
            }

            return sql.ToString();
        }

        /// <summary>
        /// 構築結果取得
        /// </summary>
        /// <param name="conn">接続オブジェクト</param>
        /// <param name="type">INSERT / UPDATE 判別用列挙子</param>
        /// <returns>生成された DbCommand</returns>
        public DbCommand Result(DbConnection conn, UpdateType type) {
            if (type == UpdateType.INSERT) {
                string query = createInsertQuery(params_);
                QueryBuilder qb = new QueryBuilder(query, params_.Values);
                return qb.Result(conn);
            }
            if (type == UpdateType.UPDATE) {
                string query = createUpdateQuery(params_);

                ICollection<object> p;
                if (whereParams_ == null || whereParams_.Count == 0 || string.IsNullOrEmpty(whereQuery_)) {
                    p = params_.Values;
                } else {
                    object[] p2 = new object[params_.Values.Count + whereParams_.Count];
                    params_.Values.CopyTo(p2, 0);
                    whereParams_.CopyTo(p2, params_.Values.Count);
                    p = p2;

                    query += @" WHERE ";
                    query += whereQuery_;
                }

                QueryBuilder qb = new QueryBuilder(query, p);
                return qb.Result(conn);

            }

            throw new InvalidOperationException(@"パラメータが不正です");
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
