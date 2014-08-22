/*
 * Copyright (c) 2006 Japan Computer Co.,Ltd.
 * Copyright (c) 2008 MACHIDA Hideki
 * Copyright (c) 2008 az'Ciel HAKKO Co.,Ltd.
 * Copyright (c) 2013 HAKKO Development Co.,Ltd. az'Ciel division.
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
 * CSV 出力クラスのテストクラス定義
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzCiel.CommonLib.IO;

namespace AzCiel.CommonLib.Test.IO {

    /// <summary>
    /// CSV出力クラスのテストクラス
    /// </summary>
    [TestClassAttribute]
    public class CVSOutputTest {

        /// <summary>
        /// 文字列クォート処理のテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestMakeColumn() {
            CSVOutput outer = new CSVOutput();

            string actVal1 = @"1-ABCD";
            string expVal1 = @"1-ABCD";
            Assert.AreEqual(expVal1, outer.MakeColumn(actVal1));

            string actVal2 = "2-\"ABCD\"";
            string expVal2 = "\"2-\"\"ABCD\"\"\"";
            Assert.AreEqual(expVal2, outer.MakeColumn(actVal2));

            string actVal3 = "3-AB\r\nCD";
            string expVal3 = "\"3-AB\nCD\"";
            Assert.AreEqual(expVal3, outer.MakeColumn(actVal3));

            string actVal4 = "4-AB\rCD";
            string expVal4 = "\"4-AB\nCD\"";
            Assert.AreEqual(expVal4, outer.MakeColumn(actVal4));

            string actVal5 = "5-AB\nCD";
            string expVal5 = "\"5-AB\nCD\"";
            Assert.AreEqual(expVal5, outer.MakeColumn(actVal5));

            string actVal6 = "6-AB\nC\"D";
            string expVal6 = "\"6-AB\nC\"\"D\"";
            Assert.AreEqual(expVal6, outer.MakeColumn(actVal6));

            string actVal7 = "7-AB,C,D";
            string expVal7 = "\"7-AB,C,D\"";
            Assert.AreEqual(expVal7, outer.MakeColumn(actVal7));

            string actVal8 = "8-A\nB,C\"D";
            string expVal8 = "\"8-A\nB,C\"\"D\"";
            Assert.AreEqual(expVal8, outer.MakeColumn(actVal8));
        }

        /// <summary>
        /// １行文字列生成のテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestMakeOneLine() {
            CSVOutput outer = new CSVOutput();

            IList<string> list1 = new List<string>();
            list1.Add("ABC");
            list1.Add("DEF");
            list1.Add("GHI");
            string expVal1 = "ABC,DEF,GHI";
            Assert.AreEqual(expVal1, outer.MakeOneLine(list1));

            IList<string> list2 = new List<string>();
            list2.Add("1\"ABC\"");
            list2.Add("2DEF");
            list2.Add("3\"GHI\"");
            string expVal2 = "\"1\"\"ABC\"\"\",2DEF,\"3\"\"GHI\"\"\"";
            Assert.AreEqual(expVal2, outer.MakeOneLine(list2));

            IList<string> list3 = new List<string>();
            list3.Add("ABC");
            list3.Add(",DEF");
            list3.Add("G,H,I");
            string expVal3 = "ABC,\",DEF\",\"G,H,I\"";
            Assert.AreEqual(expVal3, outer.MakeOneLine(list3));

            IList<string> list4 = new List<string>();
            list4.Add("ABC");
            list4.Add("\nDEF");
            list4.Add("G\nH\nI");
            string expVal4 = "ABC,\"\nDEF\",\"G\nH\nI\"";
            Assert.AreEqual(expVal4, outer.MakeOneLine(list4));

            outer.Delimitor = "\t";
            IList<string> list5 = new List<string>();
            list5.Add("ABC,");
            list5.Add("\tDEF");
            list5.Add("G\tH\tI");
            string expVal5 = "ABC,\t\"\tDEF\"\t\"G\tH\tI\"";
            Assert.AreEqual(expVal5, outer.MakeOneLine(list5));

        }

        /// <summary>
        /// CSVファイル出力のテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestWriteCsv() {
            string file = Path.GetTempFileName();
            try {
                File.Delete(file);

                using (CSVOutput outer1 = new CSVOutput(file)) {
                    IList<string>[] lists = { new List<string>(), new List<string>() };
                    lists[0].Add("あいう");
                    lists[0].Add("えおか");
                    lists[0].Add("きくけ");
                    lists[1].Add("こさし");
                    lists[1].Add("すせそ");
                    lists[1].Add("たちつ");
                    // とりあえず２行書く‘
                    outer1.WriteCsv(lists[0]);
                    outer1.WriteCsv(lists[1]);
                }

                using (CSVOutput outer2 = new CSVOutput(file, true)) {
                    string[] list = { "てとな", "にぬね", "のはひ" };
                    // １行追記
                    outer2.WriteCsv(list);
                }

                Stream stream = File.Open(file, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(stream)) {
                    byte[] buff = reader.ReadBytes((int)stream.Length);

                    Encoding enc = Encoding.GetEncoding("csWindows31J");
                    string act = enc.GetString(buff);

                    string exp = "あいう,えおか,きくけ\r\n" +
                                 "こさし,すせそ,たちつ\r\n" +
                                 "てとな,にぬね,のはひ\r\n";

                    Assert.AreEqual(exp, act);
                }
            } finally {
                File.Delete(file);
            }
        }

        /// <summary>
        /// ファイルオープンのテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestOpen() {
            // 例外をテストしてみる (既存ディレクトリと同名のファイルは作れない)
            using (CSVOutput outer3 = new CSVOutput()) {
                try {
                    outer3.Open(Path.GetTempPath());
                } catch (Exception ex) {
                    Console.Write(@"----この例外はテスト用にわざと出してるので注意" + "\n");
                    Console.Write(ex.ToString() + "\n");
                    Console.Write(@"----ここまで" + "\n");
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.Fail();
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
