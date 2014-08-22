/*
 * Copyright (c) 2006 Japan Computer Co.,Ltd.
 * Copyright (c) 2008 MACHIDA Hideki
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
 * ハッシュ Adaptor クラス定義
 */

using System;
using System.Text;
using System.Security.Cryptography;

namespace AzCiel.CommonLib.Text {

    /// <summary>
    /// 文字列を受け取り、指定のアルゴリズムでハッシュした結果を
    /// 16 進数英小文字の文字列として返却する Adapter クラス
    /// </summary>
	public class HashAdapter {

        // Hash アルゴリズムオブジェクト
        private HashAlgorithm hasher_;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hasher">ハッシュアルゴリズムオブジェクト</param>
		public HashAdapter(HashAlgorithm hasher) {
            hasher_ = hasher;
        }

        /// <summary>
        /// ハッシュ演算実行
        /// </summary>
        /// <param name="src">ハッシュする文字列</param>
        /// <returns>ハッシュ結果文字列 (16 進数英小文字)</returns>
        public string ComputeHash(string src) {
            byte[] raw = Encoding.UTF8.GetBytes(src);
            byte[] buff = hasher_.ComputeHash(raw);
            return BitConverter.ToString(buff).ToLower().Replace("-","");
        }
    }

    /// <summary>
    /// MD5 ハッシュクラス
    /// </summary>
    public class MD5Hash : HashAdapter {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MD5Hash() : base(new MD5CryptoServiceProvider()) {
        }
    }

    /// <summary>
    /// SHA1 ハッシュクラス
    /// </summary>
    public class SHA1Hash : HashAdapter {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SHA1Hash()
            : base(new SHA1CryptoServiceProvider()) {
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
