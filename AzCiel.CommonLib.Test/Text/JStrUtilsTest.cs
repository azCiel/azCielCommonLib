/*
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
 * 日本語文字列ユーティリティクラスのテストクラス
 */

using System;
using System.Text;
using AzCiel.CommonLib.Text;
using NUnit.Framework;

namespace AzCiel.CommonLib.Test.Text {

    /// <summary>
    /// 日本語文字列ユーティリティクラスのテストクラスです
    /// </summary>
    [TestFixture]
    public class JStrUtilsTest {

        /// <summary>
        /// 指定バイト数切り詰めテスト (切り詰められたか？)
        /// </summary>
        [Test]
        public void TestReduceString1() {
            Encoding encoding = Encoding.GetEncoding(@"csWindows31J");
            string instr = @"１２３４５６７";
            string expstr = @"１２３４５";

            string result = JStrUtils.ReduceString(instr, 10, encoding);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// 指定バイト数切り詰めテスト (パディングされたか？)
        /// </summary>
        [Test]
        public void TestReduceString2() {
            Encoding encoding = Encoding.GetEncoding(@"csWindows31J");
            string instr = @"１２３４５";
            string expstr = "１２３４５\0\0\0\0\0\0\0\0\0\0";

            string result = JStrUtils.ReduceString(instr, 20, encoding, '\0');

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// 指定バイト数切り詰めテスト (パディングされないか？)
        /// </summary>
        [Test]
        public void TestReduceString3() {
            Encoding encoding = Encoding.GetEncoding(@"csWindows31J");
            string instr = @"１２３４５";
            string expstr = @"１２３４５";

            string result = JStrUtils.ReduceString(instr, 20, encoding);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// 全角→半角変換テスト
        /// </summary>
        [Test]
        public void TestZen2Han() {
            string instr = @"１２３ＡＢＣ123ABCあいうアイウがぎぐガギグぱぴぷパピプう゛ヴ￥￥\";
            string expstr = @"123ABC123ABCｱｲｳｱｲｳｶﾞｷﾞｸﾞｶﾞｷﾞｸﾞﾊﾟﾋﾟﾌﾟﾊﾟﾋﾟﾌﾟｳﾞｳﾞ\\\";

            string result = JStrUtils.Zen2Han(instr);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// 半角→全角変換テスト
        /// </summary>
        [Test]
        public void TestHan2Zen() {
            string instr = @"123ABCｱｲｳｶﾞｷﾞｸﾞﾊﾟﾋﾟﾌﾟｳﾞ\\";
            string expstr = @"１２３ＡＢＣアイウガギグパピプヴ￥￥";

            string result = JStrUtils.Han2Zen(instr);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// ひらがな→カタカナ変換テスト
        /// </summary>
        [Test]
        public void TestHira2Kata() {
            string instr = @"あいうえおがぎぐげごぱぴぷぺぽう゛ウ゛ゑ";
            string expstr = @"アイウエオガギグゲゴパピプペポヴウ゛ヱ";

            string result = JStrUtils.Hira2Kata(instr);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// カタカナ→ひらがな変換テスト
        /// </summary>
        [Test]
        public void TestKata2Hira() {
            string instr = @"アイウエオガギグゲゴパピプペポヴウ゛ヱ";
            string expstr = @"あいうえおがぎぐげごぱぴぷぺぽう゛う゛ゑ";

            string result = JStrUtils.Kata2Hira(instr);

            Assert.AreEqual(expstr, result);
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
