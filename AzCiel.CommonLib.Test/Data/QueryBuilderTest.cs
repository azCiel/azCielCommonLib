/*
 * Copyright (c) 2009,2013 HAKKO Development Co.,Ltd. az'Ciel division.
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
 * DbCommand ビルダークラスのテストクラス
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using AzCiel.CommonLib.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzCiel.CommonLib.Test.Data {

    /// <summary>
    /// DbCommand ビルダークラスのテスククラス
    /// </summary>
    [TestClassAttribute]
    public class QueryBuilderTest {

        /// <summary>
        /// DbCommand ビルダーのテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestQueryBuilder() {

            string query = @"SELECT * FROM table WHERE id=? OR id=? OR id=?";
            object[] param = new object[] { 1, @"A", DateTime.Now };

            QueryBuilder builder = new QueryBuilder(query, param);

            using (SqlConnection conn = new SqlConnection())
            using (DbCommand cmd = builder.Result(conn)) {
                string exp = @"SELECT * FROM table WHERE id=@__param_0 OR id=@__param_1 OR id=@__param_2";
                Assert.AreEqual(exp, cmd.CommandText);
                Assert.AreEqual(param.Length, cmd.Parameters.Count);

                for (int i = 0; i < param.Length; i++) {
                    Assert.AreEqual(@"@__param_" + i, cmd.Parameters[i].ParameterName);
                    Assert.AreEqual(param[i], cmd.Parameters[i].Value);
                }
            }

        }

        [TestMethodAttribute]
        public void TestBuildResultSet() {

            DataSet ds = new DataSet();
            DataTable tbl = ds.Tables.Add();
            tbl.Columns.Add(@"COLUMN1", typeof(int));
            tbl.Columns.Add(@"COLUMN2", typeof(string));

            tbl.Rows.Add(new object[] { 1, @"ABC" });

            IList<SelectPabo> result = ObjectQuery.BuildResultSet<SelectPabo>(ds);
            SelectPabo pabo = result[0];

            Assert.AreEqual(1, pabo.Num);
            Assert.AreEqual(@"ABC", pabo.S);
        }

    }

    public class SelectPabo {
        private int num;
        private string s;
        [DbField(@"COLUMN1")]
        public int Num { get { return num; } set { num = value; } }
        [DbField(@"COLUMN2")]
        public string S { get { return s; } set { s = value; } }
    }

}
/*
 * -*- settings for emacs. -*-
 * Local Variables:
 * tab-width: 4
 * indent-tabs-mode: nil
 * c-basic-offset: 4
 */
