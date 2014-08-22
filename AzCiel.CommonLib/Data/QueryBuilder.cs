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
 * DbCommand のビルダークラス
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;

namespace AzCiel.CommonLib.Data {

    /// <summary>
    /// DbCommand のビルダークラス
    /// </summary>
    /// <remarks>
    /// 「?」文字を使った単純なプレースホルダを実現する
    /// </remarks>
    public class QueryBuilder {

        // クエリ文字列
        private string statement_;

        // パラメータオブジェクト列
        private IList<object> params_ = new List<object>();

        // パラメータ文字列を生成する
        private string makeParamString(int i) {
            return @"@__param_" + i;
        }

        // 「?」のプレースホルダを ADO.NET の流儀にあわせる
        private string createPlaceHolderQuery(string src) {
            int count = 0;
            string result = Regex.Replace(src, @"\?",
                                          delegate(Match match) {
                                              return makeParamString(count++);
                                          });
            return result;
        }

        /// <summary>
        /// パラメータオブジェクトを追加する
        /// </summary>
        /// <param name="o">パラメータオブジェクト</param>
        public void AddParam<T>(T o) {
            if (o is ICollection<object>) {
                AddParam((ICollection<object>)o);
            } else {
                params_.Add(o);
            }
        }

        /// <summary>
        /// パラメータオブジェクト列を追加する
        /// </summary>
        /// <param name="c">パラメータオブジェクト列</param>
        public void AddParam(ICollection<object> c) {
            if (c == null) {
                return;
            }
            foreach (object o in c) {
                AddParam(o);
            }
        }

        /// <summary>
        /// パラメータオブジェクトをクリアする
        /// </summary>
        public void ClearParam() {
            params_.Clear();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="query">「?」文字を用いたプレースホルダを含むクエリ文字列</param>
        public QueryBuilder(string query) {
            statement_ = query;
        }

        /// <summary>
        /// パラメータオブジェクトを指定したコンストラクタ
        /// </summary>
        /// <param name="query">「?」文字を用いたプレースホルダを含むクエリ文字列</param>
        /// <param name="o">パラメータオブジェクト</param>
        public QueryBuilder(string query, object o) {
            statement_ = query;
            if (o != null) {
                AddParam(o);
            }
        }

        /// <summary>
        /// パラメータオブジェクト列を指定したコンストラクタ
        /// </summary>
        /// <param name="query">「?」文字を用いたプレースホルダを含むクエリ文字列</param>
        /// <param name="c">パラメータオブジェクト列</param>
        public QueryBuilder(string query, ICollection<object> c) {
            statement_ = query;
            AddParam(c);
        }

        /// <summary>
        /// DbCommand オブジェクトを取得する
        /// </summary>
        /// <param name="conn">DB コネクションオブジェクト</param>
        /// <returns>生成された DbCommand オブジェクト</returns>
        public DbCommand Result(DbConnection conn) {
            DbCommand result = conn.CreateCommand();
            result.CommandText = createPlaceHolderQuery(statement_);
            if (params_ != null) {
                for (int i = 0; i < params_.Count; i++) {
                    DbParameter p = result.CreateParameter();
                    p.ParameterName = makeParamString(i);
                    p.Value = params_[i];
                    result.Parameters.Add(p);
                }
            }
            return result;
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
