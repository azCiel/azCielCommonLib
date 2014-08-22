﻿/*
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
 * 文字列 JOIN クラステストクラス
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzCiel.CommonLib.Text;

namespace AzCiel.CommonLib.Test.Text {

    /// <summary>
    /// 文字列 JOIN クラステストクラス
    /// </summary>
    [TestClassAttribute]
    public class StrJoinsTest {

        /// <summary>
        /// オブジェクトコレクションを文字列として連結するテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestJoin() {
            string[] instrs = { @"A", @"B", @"C" };
            string sep = @",";
            string expstr = @"A,B,C";

            string result = StrJoins.Join(sep, instrs);

            Assert.AreEqual(expstr, result);
        }

        /// <summary>
        /// オブジェクトコレクションを文字列として連結するテスト
        /// </summary>
        [TestMethodAttribute]
        public void TestJoin2() {
            string[] instrs = { @"A", @"B", @"C", @"D", @"E", @"F" };
            string sep = @",";
            string expstr = @"C,D,E";

            string result = StrJoins.Join(sep, instrs, 2, 3);

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
