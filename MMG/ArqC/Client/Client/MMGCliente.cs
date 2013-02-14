using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MMG.Config;
using System.Net.Sockets;

namespace MMG.Exec
{
   public class MMGCliente : Form
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MMGCliente));
         this._btPesquisaJogos = new System.Windows.Forms.Button();
         this._tbPortoCliente = new System.Windows.Forms.TextBox();
         this.PortoClienteLabel = new System.Windows.Forms.Label();
         this.PortoServidorLabel = new System.Windows.Forms.Label();
         this._tbPortoServidor = new System.Windows.Forms.TextBox();
         this.NickName = new System.Windows.Forms.Label();
         this._tbNickName = new System.Windows.Forms.TextBox();
         this._btLogin = new System.Windows.Forms.Button();
         this._btLog = new System.Windows.Forms.TextBox();
         this._lbListaJogos = new System.Windows.Forms.ListBox();
         this.LogLabel = new System.Windows.Forms.Label();
         this._btNorte = new System.Windows.Forms.Button();
         this._btSul = new System.Windows.Forms.Button();
         this._btEste = new System.Windows.Forms.Button();
         this._btOeste = new System.Windows.Forms.Button();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this._btAbre = new System.Windows.Forms.Button();
         this._tbIpServidor = new System.Windows.Forms.TextBox();
         this.servidorIpLabel = new System.Windows.Forms.Label();
         this._btClear = new System.Windows.Forms.Button();
         this._btSair = new System.Windows.Forms.Button();
         this._btCriarJogo = new System.Windows.Forms.Button();
         this._btJuntarJogo = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this._tbIpLocal = new System.Windows.Forms.TextBox();
         this._tbMapPath = new System.Windows.Forms.TextBox();
         this._tbConfigPath = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this._tbNomeJogo = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this._tbSalasGas = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this._tbPontuacaoAntiga = new System.Windows.Forms.TextBox();
         this._tbPontuacaoNova = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this._cbModoJogo = new System.Windows.Forms.ComboBox();
         this.label8 = new System.Windows.Forms.Label();
         this._tbNumeroSala = new System.Windows.Forms.TextBox();
         this.label9 = new System.Windows.Forms.Label();
         this._btOffLine = new System.Windows.Forms.Button();
         this._btGoOnline = new System.Windows.Forms.Button();
         this._btMudarServidor = new System.Windows.Forms.Button();
         this._btEntraNovoServidor = new System.Windows.Forms.Button();
         this._tbNovoServidor = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // _btPesquisaJogos
         // 
         this._btPesquisaJogos.Enabled = false;
         this._btPesquisaJogos.Location = new System.Drawing.Point(495, 155);
         this._btPesquisaJogos.Name = "_btPesquisaJogos";
         this._btPesquisaJogos.Size = new System.Drawing.Size(129, 23);
         this._btPesquisaJogos.TabIndex = 1;
         this._btPesquisaJogos.Text = "Pesquisa Jogos";
         this._btPesquisaJogos.Click += new System.EventHandler(this.PesquisaJogos);
         // 
         // _tbPortoCliente
         // 
         this._tbPortoCliente.Location = new System.Drawing.Point(614, 6);
         this._tbPortoCliente.Name = "_tbPortoCliente";
         this._tbPortoCliente.Size = new System.Drawing.Size(54, 20);
         this._tbPortoCliente.TabIndex = 2;
         this._tbPortoCliente.Text = "8080";
         // 
         // PortoClienteLabel
         // 
         this.PortoClienteLabel.AutoSize = true;
         this.PortoClienteLabel.Location = new System.Drawing.Point(493, 9);
         this.PortoClienteLabel.Name = "PortoClienteLabel";
         this.PortoClienteLabel.Size = new System.Drawing.Size(113, 16);
         this.PortoClienteLabel.TabIndex = 3;
         this.PortoClienteLabel.Text = "Número Porto Cliente";
         // 
         // PortoServidorLabel
         // 
         this.PortoServidorLabel.AutoSize = true;
         this.PortoServidorLabel.Location = new System.Drawing.Point(493, 38);
         this.PortoServidorLabel.Name = "PortoServidorLabel";
         this.PortoServidorLabel.Size = new System.Drawing.Size(120, 16);
         this.PortoServidorLabel.TabIndex = 4;
         this.PortoServidorLabel.Text = "Número Porto Servidor";
         // 
         // _tbPortoServidor
         // 
         this._tbPortoServidor.Location = new System.Drawing.Point(614, 38);
         this._tbPortoServidor.Name = "_tbPortoServidor";
         this._tbPortoServidor.Size = new System.Drawing.Size(54, 20);
         this._tbPortoServidor.TabIndex = 5;
         this._tbPortoServidor.Text = "8086";
         // 
         // NickName
         // 
         this.NickName.AutoSize = true;
         this.NickName.Location = new System.Drawing.Point(496, 92);
         this.NickName.Name = "NickName";
         this.NickName.Size = new System.Drawing.Size(56, 16);
         this.NickName.TabIndex = 6;
         this.NickName.Text = "NickName";
         // 
         // _tbNickName
         // 
         this._tbNickName.Location = new System.Drawing.Point(559, 89);
         this._tbNickName.Name = "_tbNickName";
         this._tbNickName.Size = new System.Drawing.Size(211, 20);
         this._tbNickName.TabIndex = 7;
         this._tbNickName.Text = "Manel";
         // 
         // _btLogin
         // 
         this._btLogin.Location = new System.Drawing.Point(683, 6);
         this._btLogin.Name = "_btLogin";
         this._btLogin.Size = new System.Drawing.Size(87, 52);
         this._btLogin.TabIndex = 8;
         this._btLogin.Text = "Fazer Login";
         this._btLogin.Click += new System.EventHandler(this.RealizaLogin);
         // 
         // _btLog
         // 
         this._btLog.Location = new System.Drawing.Point(39, 226);
         this._btLog.Multiline = true;
         this._btLog.Name = "_btLog";
         this._btLog.ReadOnly = true;
         this._btLog.Size = new System.Drawing.Size(450, 176);
         this._btLog.TabIndex = 9;
         this._btLog.Text = "";
         // 
         // _lbListaJogos
         // 
         this._lbListaJogos.Location = new System.Drawing.Point(495, 185);
         this._lbListaJogos.Name = "_lbListaJogos";
         this._lbListaJogos.Size = new System.Drawing.Size(273, 134);
         this._lbListaJogos.TabIndex = 10;
         this._lbListaJogos.SelectedIndexChanged += new System.EventHandler(this.JogoSelecionado);
         // 
         // LogLabel
         // 
         this.LogLabel.AutoSize = true;
         this.LogLabel.Location = new System.Drawing.Point(8, 226);
         this.LogLabel.Name = "LogLabel";
         this.LogLabel.Size = new System.Drawing.Size(23, 16);
         this.LogLabel.TabIndex = 11;
         this.LogLabel.Text = "Log";
         // 
         // _btNorte
         // 
         this._btNorte.Location = new System.Drawing.Point(204, 22);
         this._btNorte.Name = "_btNorte";
         this._btNorte.TabIndex = 12;
         this._btNorte.Text = "Norte";
         this._btNorte.Click += new System.EventHandler(this.goNorte);
         // 
         // _btSul
         // 
         this._btSul.Location = new System.Drawing.Point(204, 164);
         this._btSul.Name = "_btSul";
         this._btSul.TabIndex = 13;
         this._btSul.Text = "Sul";
         this._btSul.Click += new System.EventHandler(this.goSul);
         // 
         // _btEste
         // 
         this._btEste.Location = new System.Drawing.Point(349, 85);
         this._btEste.Name = "_btEste";
         this._btEste.Size = new System.Drawing.Size(75, 37);
         this._btEste.TabIndex = 14;
         this._btEste.Text = "Este";
         this._btEste.Click += new System.EventHandler(this.goEste);
         // 
         // _btOeste
         // 
         this._btOeste.Location = new System.Drawing.Point(62, 85);
         this._btOeste.Name = "_btOeste";
         this._btOeste.Size = new System.Drawing.Size(75, 37);
         this._btOeste.TabIndex = 15;
         this._btOeste.Text = "Oeste";
         this._btOeste.Click += new System.EventHandler(this.goOeste);
         // 
         // pictureBox1
         // 
         this.pictureBox1.BackColor = System.Drawing.SystemColors.MenuText;
         this.pictureBox1.Location = new System.Drawing.Point(62, 22);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(362, 165);
         this.pictureBox1.TabIndex = 16;
         this.pictureBox1.TabStop = false;
         // 
         // _btAbre
         // 
         this._btAbre.Location = new System.Drawing.Point(204, 85);
         this._btAbre.Name = "_btAbre";
         this._btAbre.Size = new System.Drawing.Size(75, 37);
         this._btAbre.TabIndex = 17;
         this._btAbre.Text = "Abre Cofre";
         this._btAbre.Click += new System.EventHandler(this.AbreCofre);
         // 
         // _tbIpServidor
         // 
         this._tbIpServidor.Location = new System.Drawing.Point(585, 63);
         this._tbIpServidor.Name = "_tbIpServidor";
         this._tbIpServidor.Size = new System.Drawing.Size(185, 20);
         this._tbIpServidor.TabIndex = 18;
         this._tbIpServidor.Text = "localhost";
         // 
         // servidorIpLabel
         // 
         this.servidorIpLabel.AutoSize = true;
         this.servidorIpLabel.Location = new System.Drawing.Point(496, 63);
         this.servidorIpLabel.Name = "servidorIpLabel";
         this.servidorIpLabel.Size = new System.Drawing.Size(76, 16);
         this.servidorIpLabel.TabIndex = 19;
         this.servidorIpLabel.Text = "IP do Servidor";
         // 
         // _btClear
         // 
         this._btClear.Location = new System.Drawing.Point(8, 242);
         this._btClear.Name = "_btClear";
         this._btClear.Size = new System.Drawing.Size(17, 160);
         this._btClear.TabIndex = 20;
         this._btClear.Text = "CLEAR";
         this._btClear.Click += new System.EventHandler(this.ClearLog);
         // 
         // _btSair
         // 
         this._btSair.Location = new System.Drawing.Point(693, 456);
         this._btSair.Name = "_btSair";
         this._btSair.TabIndex = 21;
         this._btSair.Text = "Sair";
         this._btSair.Click += new System.EventHandler(this.exit);
         // 
         // _btCriarJogo
         // 
         this._btCriarJogo.Enabled = false;
         this._btCriarJogo.Location = new System.Drawing.Point(693, 422);
         this._btCriarJogo.Name = "_btCriarJogo";
         this._btCriarJogo.TabIndex = 22;
         this._btCriarJogo.Text = "Criar Jogo";
         this._btCriarJogo.Click += new System.EventHandler(this.CriarJogo);
         // 
         // _btJuntarJogo
         // 
         this._btJuntarJogo.Enabled = false;
         this._btJuntarJogo.Location = new System.Drawing.Point(641, 155);
         this._btJuntarJogo.Name = "_btJuntarJogo";
         this._btJuntarJogo.Size = new System.Drawing.Size(129, 23);
         this._btJuntarJogo.TabIndex = 23;
         this._btJuntarJogo.Text = "Juntar a Jogo";
         this._btJuntarJogo.Click += new System.EventHandler(this.JuntarJogo);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(496, 121);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(45, 16);
         this.label1.TabIndex = 24;
         this.label1.Text = "IP Local";
         // 
         // _tbIpLocal
         // 
         this._tbIpLocal.Location = new System.Drawing.Point(559, 121);
         this._tbIpLocal.Name = "_tbIpLocal";
         this._tbIpLocal.Size = new System.Drawing.Size(209, 20);
         this._tbIpLocal.TabIndex = 25;
         this._tbIpLocal.Text = "localhost";
         // 
         // _tbMapPath
         // 
         this._tbMapPath.Location = new System.Drawing.Point(107, 422);
         this._tbMapPath.Name = "_tbMapPath";
         this._tbMapPath.Size = new System.Drawing.Size(542, 20);
         this._tbMapPath.TabIndex = 26;
         this._tbMapPath.Text = "C:\\Temp\\MMG\\ConfigFiles\\map.txt";
         // 
         // _tbConfigPath
         // 
         this._tbConfigPath.Location = new System.Drawing.Point(107, 458);
         this._tbConfigPath.Name = "_tbConfigPath";
         this._tbConfigPath.Size = new System.Drawing.Size(542, 20);
         this._tbConfigPath.TabIndex = 27;
         this._tbConfigPath.Text = "C:\\Temp\\MMG\\ConfigFiles\\config.txt";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(39, 422);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(50, 16);
         this.label2.TabIndex = 28;
         this.label2.Text = "MapPath";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(39, 458);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(60, 16);
         this.label3.TabIndex = 29;
         this.label3.Text = "ConfigPath";
         // 
         // _tbNomeJogo
         // 
         this._tbNomeJogo.Location = new System.Drawing.Point(107, 493);
         this._tbNomeJogo.Name = "_tbNomeJogo";
         this._tbNomeJogo.Size = new System.Drawing.Size(152, 20);
         this._tbNomeJogo.TabIndex = 30;
         this._tbNomeJogo.Text = "DefaultName";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(23, 497);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(78, 16);
         this.label4.TabIndex = 31;
         this.label4.Text = "Nome do Jogo";
         // 
         // _tbSalasGas
         // 
         this._tbSalasGas.Enabled = false;
         this._tbSalasGas.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this._tbSalasGas.Location = new System.Drawing.Point(73, 38);
         this._tbSalasGas.Name = "_tbSalasGas";
         this._tbSalasGas.Size = new System.Drawing.Size(39, 35);
         this._tbSalasGas.TabIndex = 32;
         this._tbSalasGas.Text = "";
         this._tbSalasGas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(42, 19);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(122, 16);
         this.label5.TabIndex = 33;
         this.label5.Text = "Número salas com Gas";
         // 
         // _tbPontuacaoAntiga
         // 
         this._tbPontuacaoAntiga.Location = new System.Drawing.Point(520, 346);
         this._tbPontuacaoAntiga.Name = "_tbPontuacaoAntiga";
         this._tbPontuacaoAntiga.Size = new System.Drawing.Size(40, 20);
         this._tbPontuacaoAntiga.TabIndex = 34;
         this._tbPontuacaoAntiga.Text = "0";
         this._tbPontuacaoAntiga.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // _tbPontuacaoNova
         // 
         this._tbPontuacaoNova.Location = new System.Drawing.Point(620, 346);
         this._tbPontuacaoNova.Name = "_tbPontuacaoNova";
         this._tbPontuacaoNova.Size = new System.Drawing.Size(44, 20);
         this._tbPontuacaoNova.TabIndex = 35;
         this._tbPontuacaoNova.Text = "0";
         this._tbPontuacaoNova.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(500, 326);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(94, 16);
         this.label6.TabIndex = 36;
         this.label6.Text = "Pontuação Antiga";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(598, 326);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(88, 16);
         this.label7.TabIndex = 37;
         this.label7.Text = "Pontuação Nova";
         // 
         // _cbModoJogo
         // 
         this._cbModoJogo.Items.AddRange(new object[] {
															"C",
															"R1",
															"R2"});
         this._cbModoJogo.Location = new System.Drawing.Point(583, 381);
         this._cbModoJogo.Name = "_cbModoJogo";
         this._cbModoJogo.Size = new System.Drawing.Size(121, 21);
         this._cbModoJogo.TabIndex = 38;
         this._cbModoJogo.Text = "R2";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(496, 385);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(76, 16);
         this.label8.TabIndex = 39;
         this.label8.Text = "Modo de Jogo";
         // 
         // _tbNumeroSala
         // 
         this._tbNumeroSala.Enabled = false;
         this._tbNumeroSala.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this._tbNumeroSala.Location = new System.Drawing.Point(363, 38);
         this._tbNumeroSala.Name = "_tbNumeroSala";
         this._tbNumeroSala.Size = new System.Drawing.Size(39, 35);
         this._tbNumeroSala.TabIndex = 40;
         this._tbNumeroSala.Text = "";
         this._tbNumeroSala.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(323, 19);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(105, 16);
         this.label9.TabIndex = 41;
         this.label9.Text = "Numero Sala Actual";
         // 
         // _btOffLine
         // 
         this._btOffLine.Enabled = false;
         this._btOffLine.Location = new System.Drawing.Point(7, 193);
         this._btOffLine.Name = "_btOffLine";
         this._btOffLine.Size = new System.Drawing.Size(179, 23);
         this._btOffLine.TabIndex = 42;
         this._btOffLine.Text = "GoOffLine";
         this._btOffLine.Click += new System.EventHandler(this.goOffLine);
         // 
         // _btGoOnline
         // 
         this._btGoOnline.Enabled = false;
         this._btGoOnline.Location = new System.Drawing.Point(309, 193);
         this._btGoOnline.Name = "_btGoOnline";
         this._btGoOnline.Size = new System.Drawing.Size(179, 23);
         this._btGoOnline.TabIndex = 43;
         this._btGoOnline.Text = "GoBackOnline";
         this._btGoOnline.Click += new System.EventHandler(this.goOnline);
         // 
         // _btMudarServidor
         // 
         this._btMudarServidor.Enabled = false;
         this._btMudarServidor.Location = new System.Drawing.Point(326, 164);
         this._btMudarServidor.Name = "_btMudarServidor";
         this._btMudarServidor.Size = new System.Drawing.Size(98, 23);
         this._btMudarServidor.TabIndex = 44;
         this._btMudarServidor.Text = "Mudar Servidor";
         this._btMudarServidor.Click += new System.EventHandler(this.MudarServidor);
         // 
         // _btEntraNovoServidor
         // 
         this._btEntraNovoServidor.Enabled = false;
         this._btEntraNovoServidor.Location = new System.Drawing.Point(280, 492);
         this._btEntraNovoServidor.Name = "_btEntraNovoServidor";
         this._btEntraNovoServidor.Size = new System.Drawing.Size(90, 23);
         this._btEntraNovoServidor.TabIndex = 45;
         this._btEntraNovoServidor.Text = "EntrarServidor";
         this._btEntraNovoServidor.Click += new System.EventHandler(this.EntraNumNovoServidor);
         // 
         // _tbNovoServidor
         // 
         this._tbNovoServidor.Location = new System.Drawing.Point(376, 493);
         this._tbNovoServidor.Name = "_tbNovoServidor";
         this._tbNovoServidor.Size = new System.Drawing.Size(194, 20);
         this._tbNovoServidor.TabIndex = 46;
         this._tbNovoServidor.Text = "127.0.0.1:8086";
         // 
         // MMGCliente
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
         this.ClientSize = new System.Drawing.Size(736, 518);
         this.Controls.Add(this._tbNovoServidor);
         this.Controls.Add(this._btEntraNovoServidor);
         this.Controls.Add(this._btMudarServidor);
         this.Controls.Add(this._btGoOnline);
         this.Controls.Add(this._btOffLine);
         this.Controls.Add(this.label9);
         this.Controls.Add(this._tbNumeroSala);
         this.Controls.Add(this.label8);
         this.Controls.Add(this._cbModoJogo);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.label6);
         this.Controls.Add(this._tbPontuacaoNova);
         this.Controls.Add(this._tbPontuacaoAntiga);
         this.Controls.Add(this.label5);
         this.Controls.Add(this._tbSalasGas);
         this.Controls.Add(this.label4);
         this.Controls.Add(this._tbNomeJogo);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this._tbConfigPath);
         this.Controls.Add(this._tbMapPath);
         this.Controls.Add(this._tbIpLocal);
         this.Controls.Add(this.label1);
         this.Controls.Add(this._btJuntarJogo);
         this.Controls.Add(this._btCriarJogo);
         this.Controls.Add(this._btSair);
         this.Controls.Add(this._btClear);
         this.Controls.Add(this.servidorIpLabel);
         this.Controls.Add(this._tbIpServidor);
         this.Controls.Add(this._btAbre);
         this.Controls.Add(this._btOeste);
         this.Controls.Add(this._btEste);
         this.Controls.Add(this._btSul);
         this.Controls.Add(this._btNorte);
         this.Controls.Add(this.LogLabel);
         this.Controls.Add(this._lbListaJogos);
         this.Controls.Add(this._btLog);
         this.Controls.Add(this._btLogin);
         this.Controls.Add(this._tbNickName);
         this.Controls.Add(this.NickName);
         this.Controls.Add(this._tbPortoServidor);
         this.Controls.Add(this.PortoServidorLabel);
         this.Controls.Add(this.PortoClienteLabel);
         this.Controls.Add(this._tbPortoCliente);
         this.Controls.Add(this._btPesquisaJogos);
         this.Controls.Add(this.pictureBox1);
         this.Name = "MMGCliente";
         this.Text = "MMG Cliente";
         this.Load += new System.EventHandler(this.MMGCliente_Load);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button _btPesquisaJogos;
      private System.Windows.Forms.TextBox _tbPortoCliente;
      private System.Windows.Forms.Label PortoClienteLabel;
      private System.Windows.Forms.Label PortoServidorLabel;
      private System.Windows.Forms.TextBox _tbPortoServidor;
      private System.Windows.Forms.Label NickName;
      private System.Windows.Forms.TextBox _tbNickName;
      private System.Windows.Forms.Button _btLogin;
      private System.Windows.Forms.TextBox _btLog;
      private System.Windows.Forms.ListBox _lbListaJogos;
      private System.Windows.Forms.Label LogLabel;
      private System.Windows.Forms.Button _btNorte;
      private System.Windows.Forms.Button _btSul;
      private System.Windows.Forms.Button _btEste;
      private System.Windows.Forms.Button _btOeste;
      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.Button _btAbre;
      private System.Windows.Forms.TextBox _tbIpServidor;
      private System.Windows.Forms.Label servidorIpLabel;
      private System.Windows.Forms.Button _btClear;
      private System.Windows.Forms.Button _btSair;
      private System.Windows.Forms.Button _btCriarJogo;
      private System.Windows.Forms.Button _btJuntarJogo;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox _tbIpLocal;
      private System.Windows.Forms.TextBox _tbMapPath;
      private System.Windows.Forms.TextBox _tbConfigPath;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox _tbNomeJogo;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox _tbSalasGas;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox _tbPontuacaoAntiga;
      private System.Windows.Forms.TextBox _tbPontuacaoNova;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.ComboBox _cbModoJogo;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.TextBox _tbNumeroSala;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Button _btOffLine;
      private System.Windows.Forms.Button _btGoOnline;
      private System.Windows.Forms.Button _btMudarServidor;
      private System.Windows.Forms.Button _btEntraNovoServidor;
      private System.Windows.Forms.TextBox _tbNovoServidor;

      //Variavel estatica que aponta para o proprio form
      public static MMGCliente form;
      //É basicamente uma ligação que existe para o servidor
      private static Cliente _cliente;
      //Sala em que o cliente se encontra
      public static RoomDesc _salaActual;
      //id do jogo em que o jogador se encontra
      public static string _idJogo;
      //Cofres ja' abertos (pa nao repetir)
      public static ArrayList _listaCofresAbertos;
      //se o cliente se encontra em modo offline ou nao
      public static bool _modoOffline = false;
      //Mapa offline
      public static MapDesc _mapaOffline;
      //Lista de cofres abertos offline
      public static ArrayList _lstCofresAbertosOffline = new ArrayList();
      //Lista de relogios vectorial do sistema que eu conheco
      public static ArrayList _lstRelogios;

      public MMGCliente()
      {
         InitializeComponent();
         form = this;
         _listaCofresAbertos = new ArrayList();
         DisableBotoesForm();
      }

      /// <summary>
      /// Login do cliente no servidor
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void RealizaLogin(object sender, EventArgs e)
      {
         bool aceite = false;
         try
         {
            _cliente = Login.regista(ref aceite, _tbPortoCliente.Text, _tbPortoServidor.Text,
                _tbIpServidor.Text, _tbNickName.Text, _tbIpLocal.Text);

            if (aceite == false)
            {
               EscreveLog("Login Rejeitado, nick já usado no servidor");
               Cliente.DestroiPorto();
               return;
            }

            //Caso tudo tenha corrido com sucesso
            MMGCliente.EscreveLog("Ligacao ao servidor com sucesso\r\n");

            //Actualiza o nickName na ligacao com o servidor
            _cliente.NickName = _tbNickName.Text;

            _btLogin.Enabled = false;

            //Torna enable a listbox que permite a pesquisa de jogos
            _lbListaJogos.Enabled = true;
            //Torna enable o butão para pesquisar jogos
            _btPesquisaJogos.Enabled = true;
            //Permite a criação de novos jogos
            _btCriarJogo.Enabled = true;

            _btMudarServidor.Enabled = true;

            //disable
            _tbIpLocal.Enabled = false;
            _tbNickName.Enabled = false;
            _tbPortoCliente.Enabled = false;
            _tbPortoServidor.Enabled = false;
            _tbIpServidor.Enabled = false;
         }
         catch (DadosInvalidosException exception)
         {
            MMGCliente.EscreveLog(exception.Message);
         }
         catch (LigacaoException ex)
         {
            MMGCliente.EscreveLog(ex.Message);
            return;
         }
         catch (System.Runtime.Remoting.RemotingException excep)
         {
            //So parar tirar o warning
            excep.Equals("");

            MMGCliente.EscreveLog("Nao foi possivel comunicar com esse Servidor... IP/Porto Errado?? ");
         }
      }

      /// <summary>
      /// Escreve no textbox de LOG (no jogo)
      /// </summary>
      /// <param name="msg"></param>
      public static void EscreveLog(string msg)
      {
         form._btLog.Text = msg + "\r\n" + form._btLog.Text;
      }

      public static void EscreveLog(string msg, int idSala)
      {
         form._btLog.Text = idSala + ":\t" + msg + "\r\n" + form._btLog.Text;
      }

      /// <summary>
      /// Apaga o log do jogo
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ClearLog(object sender, EventArgs e)
      {
         _btLog.Text = "";
      }

      private void goNorte(object sender, EventArgs e)
      {
         Comando comando = new Comando(MapDesc.NORTE, _cliente, _salaActual, _idJogo);
         comando.Executa();
         TrataBotoesMovimento(MapDesc.NORTE);
      }

      private void goSul(object sender, EventArgs e)
      {
         Comando comando = new Comando(MapDesc.SUL, _cliente, _salaActual, _idJogo);
         comando.Executa();
         TrataBotoesMovimento(MapDesc.SUL);
      }

      private void goOeste(object sender, EventArgs e)
      {
         Comando comando = new Comando(MapDesc.OESTE, _cliente, _salaActual, _idJogo);
         comando.Executa();
         TrataBotoesMovimento(MapDesc.OESTE);
      }

      private void goEste(object sender, EventArgs e)
      {
         Comando comando = new Comando(MapDesc.ESTE, _cliente, _salaActual, _idJogo);
         comando.Executa();
         TrataBotoesMovimento(MapDesc.ESTE);
      }

      private void AbreCofre(object sender, EventArgs e)
      {
         if (salaAindaNaoVisitada(_salaActual.Num) == true)
         {
            Comando comando = new Comando(Mensagem.ABRETESOURO, _cliente, _salaActual, _idJogo);
            comando.Executa();
            TrataBotoesMovimento(MapDesc.ABRIR);
            return;
         }
         //Caso já se tenha aberto este cofre
         else
         {
            EscreveLog(Configuration.TEXTO_ABRIR_COFRE_2_VEZES);
            return;
         }
      }

      /// <summary>
      /// Verifica se o cliente já abriu o cofre da sala indicada
      /// </summary>
      /// <param name="salaActual">A sala ondo o cliente se encontra</param>
      /// <returns>True caso ainda nao tenha aberto cofre.
      /// False caso tenha sido aberto</returns>
      private bool salaAindaNaoVisitada(int salaActual)
      {
         foreach (int cofre in _listaCofresAbertos)
         {
            if (salaActual == cofre)
               return false;
         }
         return true;
      }

      public static void ActualizaSalaActual(RoomDesc sala)
      {
         _salaActual = sala;
         form._tbNumeroSala.Text = "" + sala.Num;
      }

      /// <summary>
      /// Pede ao servidor a lista de jogos
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PesquisaJogos(object sender, EventArgs e)
      {
         _lbListaJogos.Items.Clear();

         ArrayList listaJogos = _cliente.PedeListaJogos();
         if (listaJogos.Count == 0)
         {
            EscreveLog("Não existem jogos no servidor");
         }

         foreach (String idjogo in listaJogos)
         {
            _lbListaJogos.Items.Add(idjogo);
         }

         return;
      }

      /// <summary>
      /// Cria um jogo no servidor
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void CriarJogo(object sender, EventArgs e)
      {
         string nomeJogo = _cbModoJogo.Text + Configuration.SEPARADOR_IDJOGO + _tbNomeJogo.Text;
         bool res = _cliente.CriaJogo(nomeJogo, _tbMapPath.Text, _tbConfigPath.Text, _cbModoJogo.Text);

         //Se o nome do jogo já existir
         if (res == false)
         {
            EscreveLog("Criação do jogo recusada");
         }
         else
         {
            EscreveLog("O jogo " + nomeJogo + " foi criado com sucesso");
         }

         return;
      }

      /// <summary>
      /// Junta o cliente ao jogo
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void JuntarJogo(object sender, EventArgs e)
      {
         _idJogo = (string)_lbListaJogos.SelectedItem;
         int salasComGas = -1;

         //Junta o cliente ao jogo (o servidor devolve a sala inicial)
         ActualizaSalaActual(_cliente.JuntarJogo(_idJogo, ref salasComGas));

         AcertaBotoesDireccoesAdequadas(_salaActual);

         //Diz o numero de salas com gas
         _tbSalasGas.Text = "" + salasComGas;


         this.Height = 437;
         this.Width = 500;
         _tbSalasGas.Enabled = true;
         _tbNumeroSala.Enabled = true;
         _btJuntarJogo.Enabled = false;
         if (JogoDoTipo(_idJogo, Configuration.MODO_R2) == true)
         {
            _btMudarServidor.Enabled = true;
            _btOffLine.Enabled = true;
         }
      }

      /// <summary>
      /// Quando alguem seleciona algum item na ListBox pesquisa Jogo
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void JogoSelecionado(object sender, EventArgs e)
      {
         _btJuntarJogo.Enabled = true;
      }

      /// <summary>
      /// Acende os botoes adequados conforme o que se puder fazer e diz se os existe aroma a veneno
      /// ou se existe um cofre sem tesouro la dentro
      /// </summary>
      /// <param name="salaActual">A sala em que o jogador se encontra</param>
      private void AcertaBotoesDireccoesAdequadas(RoomDesc salaActual)
      {
         //Norte
         if (_salaActual.North == -1)
         {
            _btNorte.Enabled = false;
         }
         else
         {
            _btNorte.Enabled = true;
         }

         //Este
         if (_salaActual.East == -1)
         {
            _btEste.Enabled = false;
         }
         else
         {
            _btEste.Enabled = true;
         }

         //Sul
         if (_salaActual.South == -1)
         {
            _btSul.Enabled = false;
         }
         else
         {
            _btSul.Enabled = true;
         }

         //Oeste
         if (_salaActual.West == -1)
         {
            _btOeste.Enabled = false;
         }
         else
         {
            _btOeste.Enabled = true;
         }

         char tipoSala = _salaActual.RoomType;
         //Sala com cofre
         if (tipoSala.Equals(MapDesc.SALAVAZIA) || tipoSala.Equals(MapDesc.SALAAROMAVENENO) || tipoSala.Equals(MapDesc.COFREVAZIO))
         {
            _btAbre.Enabled = false;
         }
         else
         {
            _btAbre.Enabled = true;
         }

         //So para DEBUG (sugerido pelo professor)
         if (tipoSala.Equals(MapDesc.SALAAROMAVENENO))
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_SALA_ONDE_EXISTIU_VENENO, _salaActual.Num);
         }
         if (tipoSala.Equals(MapDesc.COFREVAZIO))
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_COFRE_VAZIO, _salaActual.Num);
         }


      }

      /// <summary>
      /// Termina o programa
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void exit(object sender, EventArgs e)
      {
         Application.Exit();
      }

      void TrataBotoesMovimento(string movimento)
      {
         if (_modoOffline == true)
         {
            if (movimento.Equals(MapDesc.ABRIR))
            {
               _lstCofresAbertosOffline.Add(_salaActual.Num);
               _btAbre.Enabled = false;
               return;
            }
            int salaDestino = Comando.IndicaSalaDestino(movimento, _salaActual);
            _salaActual = _mapaOffline.GetSala(salaDestino);
            AcertaBotoesDireccoesAdequadas(_salaActual);
            _tbNumeroSala.Text = "" + _salaActual.Num;
            _tbSalasGas.Enabled = false;

            if (_lstCofresAbertosOffline.Contains(_salaActual.Num))
            {
               EscreveLog("Ja tentou abrir o cofre desta sala em modo offline. Só 1vez permitida", _salaActual.Num);
               _btAbre.Enabled = false;
            }
         }
         else
         {
            DisableBotoesForm();
         }
      }

      public static void TrataRespostaAbertura(int pontuacaoAntiga, int pontuacaoNova, ArrayList top10, string resultadoAccaoCliente)
      {
         form._tbPontuacaoAntiga.Text = "" + pontuacaoAntiga;
         form._tbPontuacaoNova.Text = "" + pontuacaoNova;

         if (resultadoAccaoCliente.Equals(Configuration.ACCAO_ABRIU_VENENO))
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_RESPIROU_VENENO, _salaActual.Num);
         }

         else if (resultadoAccaoCliente.Equals(Configuration.ACCAO_ABRIU_TESOURO))
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_GANHOU_OURO, _salaActual.Num);

            //Diz que ja' abri este cofre
            _listaCofresAbertos.Add(_salaActual.Num);
         }

         else if (resultadoAccaoCliente.Equals(Configuration.ACCAO_ANTECIPOU_SE_CLIENTE))
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_JOGADOR_ANTECIPOU_JOGADA, _salaActual.Num);
         }

         form.RestoreBotoesForm();
         form.AcertaBotoesDireccoesAdequadas(_salaActual);
         form._btAbre.Enabled = false;
      }

      public static void TrataRespostaMovimento(RoomDesc novaSala, int numGasRedor, int pontuacao)
      {
         ActualizaSalaActual(novaSala);
         if (_salaActual.CofreAberto == true)
         {
            MMGCliente.EscreveLog(Configuration.TEXTO_COFRE_ABERTO, novaSala.Num);
         }
         MMGCliente.form.AcertaBotoesDireccoesAdequadas(_salaActual);
         form._btMudarServidor.Enabled = true;
         form._tbSalasGas.Text = "" + numGasRedor;
      }

      public static void TrataFimJogo(int pontuacaoFinal, ArrayList Top10)
      {
         if (Top10 != null)
         {
            string imprimir = "";
            int i = 1;
            foreach (string nome in Top10)
            {
               imprimir = imprimir + i + ". " + nome + " ----> ";
               i++;
            }
            EscreveLog(imprimir);
            EscreveLog("Top 10");
            EscreveLog("Jogo TERMINOU");
         }
         form._tbPontuacaoNova.Text = "" + pontuacaoFinal;
      }

      public static void TrataActualizacaoSistema()
      {
         EscreveLog("O sistema encontra-se em actualizacao (servidor morreu) por favor repita operacao");
         form.RestoreBotoesForm();
      }

      private void goOffLine(object sender, EventArgs e)
      {
         _btOffLine.Enabled = false;
         _btGoOnline.Enabled = true;
         _btMudarServidor.Enabled = false;

         //Vou pedir o mapa do jogo
         _mapaOffline = _cliente.GoOffline(_idJogo);

         //Vou ter que parar a thread de enviar pedidos
         _modoOffline = true;
         EscreveLog("Tenho mapa e ja tou offline");
      }

      private void goOnline(object sender, EventArgs e)
      {
         _lstCofresAbertosOffline = new ArrayList();
         _modoOffline = false;
         _tbSalasGas.Enabled = true;
         _btGoOnline.Enabled = false;
         _btOffLine.Enabled = true;

         _btMudarServidor.Enabled = true;
      }

      private void MudarServidor(object sender, EventArgs e)
      {
         _btEntraNovoServidor.Enabled = true;
         _btMudarServidor.Enabled = false;
         _btCriarJogo.Enabled = false;
         _btPesquisaJogos.Enabled = false;
         DisableBotoesForm();

         _lstRelogios = _cliente.MudarServidor();

         form.Width = 802;
         form.Height = 548;
      }

      /// <summary>
      /// Quando apos se desligar de um servidor, se liga noutro
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void EntraNumNovoServidor(object sender, EventArgs e)
      {
         try
         {
            _cliente.MudarComunicacaoServidor(_tbNovoServidor.Text);
         }
         catch (LigacaoException excep)
         {
            EscreveLog(excep.Message);
            return;
         }
         EscreveLog("Consegui realizar a ligacao ao novo servidor");
         _cliente.NickName = _tbNickName.Text;
         bool servidorPronto = _cliente.VoltarEntrarServidor(_tbIpLocal.Text, _tbPortoCliente.Text, _lstRelogios);

         if (servidorPronto)
         {
            _btCriarJogo.Enabled = true;
            _btPesquisaJogos.Enabled = true;
            _btMudarServidor.Enabled = true;
            _btEntraNovoServidor.Enabled = false;
            _btCriarJogo.Enabled = true;
            _btOffLine.Enabled = true;
            if (_salaActual != null)
            {
               AcertaBotoesDireccoesAdequadas(_salaActual);
            }

            this.Height = 437;
            this.Width = 500;
         }
         else
         {
            EscreveLog("O servidor ainda nao pode garantir RYW. Tente mais tarde");
            return;
         }
      }

      /// <summary>
      /// Serve para saber a partir do id qual o tipo de jogo que é
      /// </summary>
      /// <param name="idJogo">id do jogo</param>
      /// <param name="tipo">Tipo do jogo a comparar</param>
      /// <returns></returns>
      private bool JogoDoTipo(string idJogo, string tipo)
      {
         string temp = idJogo.Split(Configuration.SEPARADOR_IDJOGO)[0];
         if (temp.Equals(tipo))
         {
            return true;
         }
         else return false;
      }

      /// <summary>
      /// Realiza disable dos botoes de movimento/abrir cofre
      /// </summary>
      private void DisableBotoesForm()
      {
         _btNorte.Enabled = false;
         _btOeste.Enabled = false;
         _btEste.Enabled = false;
         _btSul.Enabled = false;
         _btAbre.Enabled = false;
         _btMudarServidor.Enabled = false;
      }

      /// <summary>
      /// Realiza enable dos botoes de movimento/abrir cofre
      /// </summary>
      private void RestoreBotoesForm()
      {
         _btNorte.Enabled = true;
         _btOeste.Enabled = true;
         _btEste.Enabled = true;
         _btSul.Enabled = true;
         _btAbre.Enabled = true;
         _btMudarServidor.Enabled = true;
         Cursor.Current = Cursors.Default;
      }

      //Comentado para ser compativel com versao .NET 1.3
      /*
      private void InitializeComponent()
      {
         // 
         // MMGCliente
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Name = "MMGCliente";
         this.Load += new System.EventHandler(this.MMGCliente_Load);

      }*/

      private void MMGCliente_Load(object sender, System.EventArgs e)
      {

      }
   }
}
