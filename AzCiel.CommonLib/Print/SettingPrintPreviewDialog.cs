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
 * ページ設定 / 印刷ダイアログに対応したプレビューダイアログ
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace AzCiel.CommonLib.Print {
    
    /// <summary>
    /// ページ設定 / 印刷ダイアログに対応したプレビューダイアログ
    /// </summary>
    public class SettingPrintPreviewDialog : System.Windows.Forms.PrintPreviewDialog {

        // コンポーネント
        private IContainer components_;
        // ToolStrip 用イメージリスト
        private ImageList imageList_;
        // 印刷ボタン
        private ToolStripButton toolStripButtonPrint_ = null;
        // ページ設定ボタン
        private ToolStripButton toolStripButtonPageSetup_ = null;
        // レイアウトボタンの表示状態
        private bool isLayoutButtonsVisible_ = true;

        /// <summary>
        /// ページ設定ダイアログ
        /// </summary>
        public PageSetupDialog PageSetupDialog;

        /// <summary>
        /// 印刷ダイアログ
        /// </summary>
        public PrintDialog PrintDialog;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingPrintPreviewDialog() {
            InitializeComponent();
        }

        // 印刷ボタンの付け替え / ページ設定ボタン追加
        private void buttonsCustomize() {
            ToolStrip tools = (ToolStrip)Controls[@"toolStrip1"];
            if (toolStripButtonPrint_ == null) {
                ToolStripItem oldItem = (ToolStripItem)tools.Items[0];
                toolStripButtonPrint_ = new ToolStripButton(oldItem.Text, (Image)oldItem.Image.Clone(),
                                                           new EventHandler(SettingPrintPreviewDialog_toolStripButtonPrintClick));
                toolStripButtonPrint_.DisplayStyle = oldItem.DisplayStyle;
                tools.Items.Insert(0, toolStripButtonPrint_);
                tools.Items.Remove(oldItem);
                oldItem.Dispose();
            }

            if (toolStripButtonPageSetup_ == null) {
                toolStripButtonPageSetup_ = new ToolStripButton(@"ページ設定", imageList_.Images[0],
                                                    new EventHandler(SettingPrintPreviewDialog_toolStripButtonPageSetupClick));
                toolStripButtonPageSetup_.ImageTransparentColor = imageList_.TransparentColor;
                toolStripButtonPageSetup_.DisplayStyle = ToolStripItemDisplayStyle.Image;
                tools.Items.Insert(0, toolStripButtonPageSetup_);
            }
        }

        // コンポーネント初期化
        private void InitializeComponent() {
            this.components_ = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingPrintPreviewDialog));
            this.imageList_ = new System.Windows.Forms.ImageList(this.components_);
            this.PageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.PrintDialog = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList_.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList_.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList_.Images.SetKeyName(0, "PrintSetup");
            // 
            // PrintDialog
            // 
            this.PrintDialog.UseEXDialog = true;
            // 
            // SettingPrintPreviewDialog
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "SettingPrintPreviewDialog";
            this.ResumeLayout(false);

            // !!! デザインモードで編集する場合はこの行をコメントアウト !!!
            // !!! 後で戻すのを忘れないように
            buttonsCustomize();
        }

        // ページ設定ボタンハンドラ
        private void SettingPrintPreviewDialog_toolStripButtonPageSetupClick(object sender, EventArgs e) {
            if (Document == null) {
                return;
            }

            PageSetupDialog.EnableMetric = true;
            PageSetupDialog.Document = Document;
            PrintDialog.PrinterSettings = Document.PrinterSettings;
            if (PageSetupDialog.ShowDialog() == DialogResult.OK) {
                Document.PrinterSettings = (PrinterSettings)PageSetupDialog.PrinterSettings.Clone();
                Document.PrinterSettings.PrintRange = PrintRange.AllPages;
                PrintPreviewControl.InvalidatePreview();
            }

        }

        // 印刷ボタンハンドラ
        private void SettingPrintPreviewDialog_toolStripButtonPrintClick(object sender, EventArgs e) {
            if (Document == null) {
                return;
            }

            if (Document.PrinterSettings == null) {
                Document.PrinterSettings = new PrinterSettings();
            }
            PrintDialog.PrinterSettings = Document.PrinterSettings;
            PrintDialog.Document = Document;
            PrintDialog.PrinterSettings = Document.PrinterSettings;
            if (PrintDialog.ShowDialog() == DialogResult.OK) {
                Document.Print();
                Document.PrinterSettings.PrintRange = PrintRange.AllPages;
                PrintPreviewControl.InvalidatePreview();
            }
        }

        /// <summary>
        /// レイアウトボタンの表示状態
        /// </summary>
        [Browsable(true),
         Category(@"Behavior"),
         DefaultValue(true),
         Description(@"レイアウトボタンの表示、非表示を示します")
        ]
        public bool LayoutButtonsVisible {
            get {
                return isLayoutButtonsVisible_;
            }
            set {
                ToolStrip tools = (ToolStrip)Controls[@"toolStrip1"];
                for (int i = 3; i <= 8; i++) {
                    tools.Items[i].Visible = value;
                }
                isLayoutButtonsVisible_ = value;
            }
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
