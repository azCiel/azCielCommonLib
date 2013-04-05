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
 * CSV 出力クラス定義
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace AzCiel.CommonLib.IO {

    /// <summary>
    /// CSVファイル出力クラス
    /// </summary>
    public class CSVOutput : IDisposable {

        // 各プロパティのデフォルト値
        private const string DEFAULT_CHARSET_ = @"csWindows31J";
        private const string DEFAULT_DELIMITOR_ = @",";
        private const string DEFAULT_LINEFEED_ = "\r\n";

        // 出力文字セット
        private string charset_    = DEFAULT_CHARSET_;
        // デリミタ (カラム区切り文字)
        private string delimitor_ = DEFAULT_DELIMITOR_;
        // 改行コード
        private string linefeed_  = DEFAULT_LINEFEED_;

        // ファイル出力オブジェクト
        private BinaryWriter writer_ = null;
        // エンコーダ
        private Encoding encoder_ = Encoding.GetEncoding(DEFAULT_CHARSET_);

        /// <summary>
        /// ファイル出力文字セット
        /// </summary>
        /// <remarks>
        /// <para>
        /// CSVファイルの出力文字セット。
        /// Encoding#GetEncoding() で指定できるものでなければならない
        /// </para>
        /// <para>
        /// デフォルトは「csWindows31J」
        /// </para>
        /// </remarks>
        public string Charset {
            get {
                return charset_;
            }
            set {
                charset_ = value;
                encoder_ = Encoding.GetEncoding(charset_);
            }
        }

        /// <summary>
        /// カラムデリミタ文字列
        /// </summary>
        /// <remarks>
        /// <para>
        /// CSVデータ１行中の各カラムを区切る文字列
        /// </para>
        /// <para>
        /// デフォルトは「,」
        /// </para>
        /// </remarks>
        public string Delimitor {
            get {
                return delimitor_;
            }
            set {
                delimitor_ = value;
            }
        }

        /// <summary>
        /// 改行コード文字列
        /// </summary>
        /// <remarks>
        /// <para>
        /// CSVファイルの各行における改行文字列
        /// </para>
        /// <para>
        /// デフォルトは「\r\n」
        /// </para>
        /// </remarks>
        public string Linefeed {
            get {
                return linefeed_;
            }
            set {
                linefeed_ = value;
            }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public CSVOutput() {
        }

        /// <summary>
        /// ファイルパスを指定したコンストラクタ
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <remarks>
        /// <para>
        /// 指定されたファイルを上書きモードで開く
        /// </para>
        /// <para>
        /// 異常発生時には File.Open() と同じ例外が throw される
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">異常発生時</exception>
        public CSVOutput(string filePath) : this(filePath, false) {
        }

        /// <summary>
        /// ファイルパスと追記モードを指定したコンストラクタ
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <param name="isAppend">追記モード (true=追記 / false=上書き)</param>
        /// <remarks>
        /// <para>
        /// 指定されたファイルを、isAppend が true なら追記モード、
        /// false なら上書きモードで開く
        /// </para>
        /// <para>
        /// 異常発生時には File.Open() と同じ例外が throw される
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">異常発生時</exception>
        public CSVOutput(string filePath, bool isAppend) {
            Open(filePath, isAppend);
        }

        /// <summary>
        /// 既に開いているストリームを指定したコンストラクタ
        /// </summary>
        /// <param name="stream">ストリームオブジェクト</param>
        /// <remarks>
        /// <para>
        /// 指定されたストリームに CSV を出力するコンストラクタ
        /// </para>
        /// <para>
        /// Close() または Dispose() が呼ばれると、このストリームを
        /// 閉じてしまうので注意
        /// </para>
        /// </remarks>
        public CSVOutput(Stream stream) {
            AttachStream(stream);
        }

        /// <summary>
        /// アンマネージドリソースを開放する
        /// </summary>
        /// <remarks>
        /// 開いているファイルを閉じる
        /// </remarks>
        public void Dispose() {
            Close();
        }

        /// <summary>
        /// 指定されたファイルを開く
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <remarks>
        /// <para>
        /// 指定されたファイルを上書きモードで開く
        /// </para>
        /// <para>
        /// 異常発生時には File.Open() と同じ例外が throw される
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">異常発生時</exception>
        public void Open(string filePath) {
            Open(filePath, false);
        }

        /// <summary>
        /// 指定されたファイルを指定された追記モードで開く
        /// </summary>
        /// <param name="filePath">出力ファイルパス</param>
        /// <param name="isAppend">追記モード (true=追記 / false=上書き)</param>
        /// <remarks>
        /// <para>
        /// 指定されたファイルを、isAppend が true なら追記モード、
        /// false なら上書きモードで開く
        /// </para>
        /// <para>
        /// 異常発生時には File.Open() と同じ例外が throw される
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">異常発生時</exception>
        public void Open(string filePath, bool isAppend) {
            if (writer_ != null) {
                Close();
            }
            FileMode mode = isAppend ? FileMode.Append : FileMode.Create;
            Stream stream = new BufferedStream(File.Open(filePath, mode));
            writer_ = new BinaryWriter(stream);
        }

        /// <summary>
        /// 既に開いている Stream を関連付ける
        /// </summary>
        /// <param name="stream">ストリームオブジェクト</param>
        /// <remarks>
        /// <para>
        /// 既に開いている Stream に CSV 出力すよう関連付ける。
        /// </para>
        /// <para>
        /// Close() 及び Dispose() でストリームが閉じられてしまうので
        /// 注意すること。
        /// </para>
        /// </remarks>
        public void AttachStream(Stream stream) {
            if (writer_ != null) {
                Close();
            }
            writer_ = new BinaryWriter(stream);
        }

        /// <summary>
        /// 開いているファイルを閉じる
        /// </summary>
        public void Close() {
            if (writer_ != null) {
                try {
                    writer_.Close();
                } finally {
                    writer_ = null;
                }
            }
        }

        // いろんな改行コードを "\n" に統一する
        private string nlConvert(string src) {
            string result = src;
            result = result.Replace("\r\n", "\n");
            result = result.Replace("\r", "\n");
            return result;
        }

        /// <summary>
        /// 指定された文字列を CSV 出力可能なように加工 (クォート) する
        /// </summary>
        /// <param name="src">文字列</param>
        /// <returns>加工された文字列</returns>
        public string MakeColumn(string src) {
            string data = nlConvert(src);

            bool hasNl = data.IndexOf('\n') > -1;
            bool hasDelm = data.IndexOf(delimitor_) > -1;
            bool hasQuote = data.IndexOf('"') > -1;

            if (hasQuote) {
                // 「"」を「""」とする
                data = data.Replace("\"", "\"\"");
            }

            StringBuilder builder = new StringBuilder();
            if (hasNl || hasDelm || hasQuote) {
                builder.Append('"');
                builder.Append(data);
                builder.Append('"');
            } else  {
                builder.Append(data);
            }
            return builder.ToString();
        }

        /// <summary>
        /// CSV １行分の文字列を生成
        /// </summary>
        /// <remarks>
        /// ICollection に格納されたオブジェクト列を文字列化し、
        /// CSV の１行分の文字列に変換する
        /// </remarks>
        /// <param name="record">オブジェクト列</param>
        /// <returns>CSV １行分の文字列</returns>
        public string MakeOneLine<T>(ICollection<T> record) {
            StringBuilder builder = new StringBuilder();
            int count = 0;

            foreach (object item in record) {
                string field = item.ToString();
                builder.Append(MakeColumn(field));
                count++;

                if (count < record.Count) {
                    builder.Append(delimitor_);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// CSV へ１行分のデータを書き込む
        /// </summary>
        /// <param name="record">オブジェクト列</param>
        /// <remarks>
        /// <para>
        /// ICollection に格納されたオブジェクト列を文字列化し、
        /// Charset で指定された文字セットでファイルに書き込む
        /// </para>
        /// <para>
        /// ファイルを開いていないときには NullReferenceException が出るので注意!
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">異常発生時</exception>
        public void WriteCsv<T>(ICollection<T> record) {
            string str = MakeOneLine(record);
            if (linefeed_ != "\n") {
                str = str.Replace("\n", linefeed_);
            }

            byte[] buff = encoder_.GetBytes(str);
            writer_.Write(buff);
            writer_.Write(encoder_.GetBytes(linefeed_));
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
