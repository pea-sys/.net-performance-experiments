namespace GenPromote
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button = new System.Windows.Forms.Button();
            this.LatencyModeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(12, 34);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(96, 28);
            this.button.TabIndex = 0;
            this.button.Text = "start";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // LatencyModeLabel
            // 
            this.LatencyModeLabel.AutoSize = true;
            this.LatencyModeLabel.Location = new System.Drawing.Point(10, 9);
            this.LatencyModeLabel.Name = "LatencyModeLabel";
            this.LatencyModeLabel.Size = new System.Drawing.Size(72, 12);
            this.LatencyModeLabel.TabIndex = 1;
            this.LatencyModeLabel.Text = "LatencyMode";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 86);
            this.Controls.Add(this.LatencyModeLabel);
            this.Controls.Add(this.button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Label LatencyModeLabel;
    }
}

