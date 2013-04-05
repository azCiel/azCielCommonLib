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
 * 日本語文字列ユーティリティクラス
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

namespace AzCiel.CommonLib.Text {

    /// <summary>
    /// 日本語文字列ユーティリティクラス
    /// </summary>
    public static class JStrUtils {

        /// <summary>
        /// 文字列を指定エンコーディングに変換した際に、指定バイト数以内になるように切り詰める
        /// </summary>
        /// <remarks>
        /// 入力文字列が指定バイト数より短かった場合は、入力文字列そのものを返却する
        /// </remarks>
        /// <param name="src">入力文字列</param>
        /// <param name="byteLen">バイト数</param>
        /// <param name="encoding">エンコーディングオブジェクト</param>
        /// <returns>変換結果文字列</returns>
        public static string ReduceString(string src, int byteLen, Encoding encoding) {
            if (string.IsNullOrEmpty(src)) {
                return @"";
            }
            string result = src;
            while (encoding.GetByteCount(result) > byteLen) {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        ///  文字列を指定エンコーディングに変換した際に、指定バイト数以内になるように切り詰める
        /// </summary>
        /// <remarks>
        /// 入力文字列が指定バイト数より短かった場合は、指定バイト数になるまで文字 padChar
        /// を連結する
        /// </remarks>
        /// <param name="src">入力文字列</param>
        /// <param name="byteLen">バイト数</param>
        /// <param name="encoding">エンコーディングオブジェクト</param>
        /// <param name="padChar">パディング文字</param>
        /// <returns>変換結果文字列</returns>
        public static string ReduceString(string src, int byteLen, Encoding encoding, char padChar) {
            StringBuilder result = new StringBuilder(ReduceString(src, byteLen, encoding));
            while (encoding.GetByteCount(result.ToString()) < byteLen) {
                result.Append(padChar);
            }
            return result.ToString();
        }

        /// <summary>
        /// 文字列に含まれる全角文字を半角文字に変換する
        /// </summary>
        /// <remarks>
        /// 全角ひらがなは半角カタカナに変換される
        /// </remarks>
        /// <param name="src">入力文字列</param>
        /// <returns>変換結果文字列</returns>
        public static string Zen2Han(string src) {
            if (string.IsNullOrEmpty(src)) {
                return @"";
            }
            string result = Strings.StrConv(src, VbStrConv.Katakana, 0);
            result = Strings.StrConv(result, VbStrConv.Narrow, 0);
            return result;
        }

        /// <summary>
        /// 文字列に含まれる半角文字を全角文字に変換する
        /// </summary>
        /// <remarks>
        /// 「\」も全角に変換するので、ファイルパス文字列を渡さないように注意
        /// </remarks>
        /// <param name="src">入力文字列</param>
        /// <returns>変換結果文字列</returns>
        public static string Han2Zen(string src) {
            if (string.IsNullOrEmpty(src)) {
                return @"";
            }
            string result = Strings.StrConv(src, VbStrConv.Wide, 0);
            result = result.Replace('\\', '￥');
            return result;
        }

        /// <summary>
        /// 文字列に含まれるひらがなをカタカナに変換する
        /// </summary>
        /// <param name="src">入力文字列</param>
        /// <returns>変換結果文字列</returns>
        public static string Hira2Kata(string src) {
            if (string.IsNullOrEmpty(src)) {
                return @"";
            }
            string result = src.Replace(@"う゛", @"ヴ");
            result = Strings.StrConv(result, VbStrConv.Katakana, 0);
            return result;
        }

        /// <summary>
        /// 文字列に含まれるカタカナをひらがなに変換する
        /// </summary>
        /// <param name="src">入力文字列</param>
        /// <returns>変換結果文字列</returns>
        public static string Kata2Hira(string src) {
            if (string.IsNullOrEmpty(src)) {
                return @"";
            }
            string result = Strings.StrConv(src, VbStrConv.Hiragana, 0);
            result = result.Replace(@"ヴ", @"う゛");
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
