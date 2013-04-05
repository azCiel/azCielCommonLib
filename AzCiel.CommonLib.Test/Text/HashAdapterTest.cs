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
 * ハッシュ Adapter クラスのテストクラス
 */

using System;
using AzCiel.CommonLib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzCiel.CommonLib.Test.Text {

    /// <summary>
    /// ハッシュ Adapter クラスのテストクラスです
    /// </summary>
    [TestClassAttribute]
    public class HashAdapterTest {

        /// <summary>
        /// MD5ハッシュのテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestMD5() {
            HashAdapter hash = new MD5Hash();
            string src1 = @"ABC";
            string result1 = hash.ComputeHash(src1);
            Assert.AreEqual(32, result1.Length);
            string hashed1 = @"902fbdd2b1df0c4f70b4a5d23525e932";
            Assert.AreEqual(hashed1, result1);

            // 日本語を含む文字列は？
            string src2 = @"あいう";
            string result2 = hash.ComputeHash(src2);
            Assert.AreEqual(32, result2.Length);
            string hashed2 = @"df5c588826b00952db2ff6c8829cb086";
            Assert.AreEqual(hashed2, result2);
        }

        /// <summary>
        /// SHA1ハッシュのテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestSHA1() {
            HashAdapter hash = new SHA1Hash();
            string src1 = @"ABC";
            string result1 = hash.ComputeHash(src1);
            Assert.AreEqual(40, result1.Length);
            string hashed1 = @"3c01bdbb26f358bab27f267924aa2c9a03fcfdb8";
            Assert.AreEqual(hashed1, result1);

            // 日本語を含む文字列は？
            string src2 = @"あいう";
            string result2 = hash.ComputeHash(src2);
            Assert.AreEqual(40, result2.Length);
            string hashed2 = @"eb636ba7c320e00b3749ad404b7adc7609560dee";
            Assert.AreEqual(hashed2, result2);
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
