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
 * System.String#Join() が不便なのでもっと柔軟にしたクラス
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace AzCiel.CommonLib.Text {

    /// <summary>
    /// System.String#Join() が不便なのでもっと柔軟にしたクラス
    /// </summary>
    /// <remarks>
    /// C# 3.0 だったら String クラスの拡張メソッドにできるかも？
    /// </remarks>
    public static class StrJoins {

        /// <summary>
        /// オブジェクトコレクションを separator で連結する
        /// </summary>
        /// <param name="separator">区切り文字列</param>
        /// <param name="values">連結するオブジェクトコレクション</param>
        /// <param name="startIndex">オブジェクトコレクションの開始インデックス</param>
        /// <param name="count">連結するオブジェクトコレクションの数</param>
        /// <returns>結果文字列</returns>
        public static string Join<T>(string separator, ICollection<T> values, int startIndex, int count) {
            StringBuilder result = new StringBuilder();

            int i = 0;
            int cnt = 0;
            foreach (object o in values) {
                i++;
                if (i <= startIndex) {
                    continue;
                }
                if (cnt >= count) {
                    break;
                }
                if ((i - 1) > startIndex) {
                    result.Append(separator);
                }
                result.Append(o.ToString());
                cnt++;
            }

            return result.ToString();
        }

        /// <summary>
        /// オブジェクトコレクションを separator で連結する
        /// </summary>
        /// <param name="separator">区切り文字列</param>
        /// <param name="values">連結するオブジェクトコレクション</param>
        /// <returns>結果文字列</returns>
        public static string Join<T>(string separator, ICollection<T> values) {
            return StrJoins.Join(separator, values, 0, values.Count);
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
