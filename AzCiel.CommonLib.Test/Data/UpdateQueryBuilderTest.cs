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
 * 更新用 DbCommand ビルダークラスのテストクラス
 */

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using AzCiel.CommonLib.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzCiel.CommonLib.Test.Data {

    /// <summary>
    /// 更新用 DbCommand ビルダークラスのテストクラス
    /// </summary>
    [TestClassAttribute]
    public class UpdateQueryBuilderTest {

        /// <summary>
        /// INSERT 文生成テスト
        /// </summary>
        [TestMethodAttribute]
        public void TestUpdateQueryBuilder1() {
            IDictionary<string, object> param = new Dictionary<string, object>();
            param.Add(@"COLUMN1", 1);
            param.Add(@"COLUMN2", @"ABC");
            string tbl = @"TableName";

            string exp = @"INSERT INTO TableName (COLUMN1,COLUMN2) VALUES (@__param_0,@__param_1)";

            UpdateQueryBuilder b = new UpdateQueryBuilder(tbl);
            b.SetParams(param);
            using (SqlConnection conn = new SqlConnection())
            using (DbCommand result = b.Result(conn, UpdateType.INSERT)) {
                Assert.AreEqual(exp, result.CommandText);

                Assert.AreEqual(1, result.Parameters[0].Value);
                Assert.AreEqual(@"ABC", result.Parameters[1].Value);
            }
        }

        /// <summary>
        /// UPDATE 文生成テスト
        /// </summary>
        [TestMethodAttribute]
        public void TestUpdateQueryBuilder2() {
            IDictionary<string, object> param = new Dictionary<string, object>();
            param.Add(@"COLUMN1", 1);
            param.Add(@"COLUMN2", @"ABC");
            string tbl = @"TableName";

            object[] whereParam = { 2, @"DEF" };
            string where = @"COLUMN3=? AND COLUMN4=?";

            string exp = @"UPDATE TableName SET COLUMN1=@__param_0,COLUMN2=@__param_1 WHERE COLUMN3=@__param_2 AND COLUMN4=@__param_3";

            UpdateQueryBuilder b = new UpdateQueryBuilder(tbl);
            b.SetParams(param);
            b.SetWhere(where, whereParam);

            using (SqlConnection conn = new SqlConnection())
            using (DbCommand result = b.Result(conn, UpdateType.UPDATE)) {
                Assert.AreEqual(exp, result.CommandText);

                Assert.AreEqual(1, result.Parameters[0].Value);
                Assert.AreEqual(@"ABC", result.Parameters[1].Value);
                Assert.AreEqual(2, result.Parameters[2].Value);
                Assert.AreEqual(@"DEF", result.Parameters[3].Value);
            }
        }

        /// <summary>
        /// INSERT 文生成テスト
        /// </summary>
        [TestMethodAttribute]
        public void TestUpdateQueryBuilder3() {

            string tbl = @"TableName";
            string exp = @"INSERT INTO TableName (COLUMN1,COLUMN2) VALUES (@__param_0,@__param_1)";

            UpdateQueryBuilder b = new UpdateQueryBuilder(tbl);
            UpdatePabo pabo = new UpdatePabo();
            b.SetParams(ObjectQuery.GetParams(pabo));

            using (SqlConnection conn = new SqlConnection())
            using (DbCommand result = b.Result(conn, UpdateType.INSERT)) {
                Assert.AreEqual(exp, result.CommandText);

                Assert.AreEqual(pabo.Num, result.Parameters[0].Value);
                Assert.AreEqual(pabo.S, result.Parameters[1].Value);
            }
        }

        /// <summary>
        /// UPDATAE 文生成テスト
        /// </summary>
        [TestMethodAttribute]
        public void TestUpdateQueryBuilder4() {

            object[] whereParam = { 2, @"DEF" };
            string where = @"COLUMN3=? AND COLUMN4=?";
            string tbl = @"TableName";

            string exp = @"UPDATE TableName SET COLUMN1=@__param_0,COLUMN2=@__param_1 WHERE COLUMN3=@__param_2 AND COLUMN4=@__param_3";

            UpdateQueryBuilder b = new UpdateQueryBuilder(tbl);
            UpdatePabo pabo = new UpdatePabo();
            b.SetParams(ObjectQuery.GetParams(pabo));
            b.SetWhere(where, whereParam);

            using (SqlConnection conn = new SqlConnection())
            using (DbCommand result = b.Result(conn, UpdateType.UPDATE)) {
                Assert.AreEqual(exp, result.CommandText);

                Assert.AreEqual(1, result.Parameters[0].Value);
                Assert.AreEqual(@"ABC", result.Parameters[1].Value);
                Assert.AreEqual(2, result.Parameters[2].Value);
                Assert.AreEqual(@"DEF", result.Parameters[3].Value);
            }
        }

    }

    public class UpdatePabo {
        private int num = 1;
        private string s = @"ABC";
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
