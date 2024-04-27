# RoboCopyMan
---

## Release v1.0.0-beta.4 (2024.04.28)
### 機能追加とバグ修正
#### バグ修正
- Aboutダイアログが何度も開けるのを修正。
#### 機能追加
- 結果ダイアログのメニューで、選択したバックアップ設定ファイルをエディタで開く機能を追加を追加。
（RoboCopyMan を再起動しないと反映されないです）
- 結果ダイアログのメニューで、選択したバックアップ設定ファイルの設定で生成される robocopy コマンド をクリップボードにコピーする機能を追加。

---
## 何をするものか？

- robocopy コマンドを定期的に実行するだけのソフトです。
でも案外このシンプルさを持つソフトは世間にないので自分では重宝しています。

> ふだん Dropbox を作業フォルダに使用しているのですが心配性なので、定期的にローカルの NAS にバックアップをとりたいなと思い
いろいろ試してみました。
> しかしながら、バッチファイルは窓が消せなかったり、タスクスケジューラでやろうにもパスワードレスのMicrosoftアカウントではやっぱり窓が消せなかったり、
その他あるかもしれないのですが、結局よくわからないのでこのソフトを作りました。

> ちなみに、RoboCopyScheduler というフリーソフトがあるのですが、それが私の環境ではうまく使えなかったんですよね・・・
> RoboCopyScheduler がうまく使える方はそちらを使用した方がよいです。RoboCopyScheduler の方が機能が豊富です。

## インストール、アンインストール

- 実行ファイル RoboCopyMan.exe を実行すれば OK です。
- レジストリは使用していないので、フォルダごと削除で OK です。

## 注意

- 最低限の機能しか実装していません。ライセンスはフリーなので、勝手にいじってくださいませ・・・
- 不明点はコード見た方が早いかも？
- 自己責任ではありますが事故ると心苦しいので、まずは消えてもよいファイルで試して使ってください。

## 使い方

- settings フォルダの中に、バックアップ情報を書き込んだ toml ファイルを格納しておきます。

- x-drive.toml、x-drive.toml が入ってますが、これは私の設定ファイルですので参考にして製作してください。
変更するポイントは、title、srcDir、dstDir、logDir でよいかと思います。

## 設定ファイルの書き方

- title は、好きな名前をつけれます。
- srcDir は、コピー元
- dstDir は、コピー先
- logDir は、ログの出力先になります。実際のログファイル名は、dstDir の 最後のフォルダ名 に 日付 をつけたファイル名で保存されます。
（MainPC-XDrive-20240427-190427.txt のような感じです。）
- logDatetimeFmt は、ログファイルの日付フォーマットです。C#の日付フォーマットに準拠しています。

- intervalMinutes は、バックアップの実行間隔で、この時間ごとに robocopy 実行されます。
- delayMinutes は、アプリ起動後初回の実行時間です。
（例えば、ネットワークドライブの接続をまつなど）

- option は、オプション設定です。
(例えば・・・"/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE")
- exclude は、除外するファイルです。
(例えば・・・ "\"System Volume Information\" \"$RECYCLE.BIN\"")

### 設定ファイル例
```
# -*- coding: utf-8 -*-

# バックアップ設定のタイトル
title = "MainPC X Drive Backup"

# バックアップ元ディレクトリ
srcDir = "X:"
# バックアップ先ディレクトリ
dstDir = "M:\\backups\\main-pc-daily\\MainPC-XDrive"
# ログファイルの出力先ディレクトリ
logDir = "M:\\backups\\main-pc-daily\\logs"
# ログファイル名に使用する日付フォーマット (C#の日付フォーマットに準拠)
logDatetimeFmt = "yyyyMMdd-HHmmdd"

# robocopy のオプション
option = "/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE"
# 除外するファイル・ディレクトリ
xdFiles = "\"System Volume Information\" \"$RECYCLE.BIN\""

# バックアップ間隔（分）
intervalMinutes = 60
# 初回実行を遅らせるディレイ（分）
delayMinutes = 5

# 生成される robocopy のコマンド例
# LogFilePath = {LogDir}\\{dstDir のディレクトリ名}-{logDatetimeFmt}.txt
# robocopy {SrcDir} {DstDir} /LOG:{LogFilePath} /XD {XdFiles} {Option}
```

### 上記設定で実行されるコマンド
```
robocopy X: M:\backups\main-pc-daily\MainPC-XDrive /LOG:M:\backups\main-pc-daily\logs\MainPC-XDrive-20240427-200427.txt /XD "System Volume Information" "$RECYCLE.BIN" /MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE
```

### 上記設定での robocopy ログファイルの例
```
-------------------------------------------------------------------------------
   ROBOCOPY     ::     Windows の堅牢性の高いファイル コピー                              
-------------------------------------------------------------------------------

  開始: 2024年4月27日 19:24:37
   コピー元 = X:\
     コピー先 : M:\backups\main-pc-daily\MainPC-XDrive\

    ファイル: *.*
	    
 除外ディレクトリ: System Volume Information
	    $RECYCLE.BIN
	    
  オプション: *.* /FFT /TEE /S /E /DCOPY:DAT /COPY:DAT /PURGE /MIR /NP /XJF /XJD /MT:128 /R:1 /W:10 

------------------------------------------------------------------------------


------------------------------------------------------------------------------

                  合計     コピー済み      スキップ       不一致        失敗    Extras
   ディレクトリ:      1869      1869      1869         0         0         0
     ファイル:     10940         0     10940         0         0         0
      バイト:  26.725 g         0  26.725 g         0         0         0
       時刻:   0:00:03   0:00:00                       0:00:00   0:00:03
   終了: 2024年4月27日 19:24:40
```

## アプリの動作

- 起動するとタスクトレイにごせん像アイコンが表示されます。これが本体です。
- ごせん像を右クリックでメニュー、ダブクリでバックアップ結果を表示するためのダイアログが表示されます。
- バックアップ結果ダイアログのリスト項目をダブルクリックすると、そのバックアップ設定に設定されているログファイルフォルダを開きます。
- ちなみに、アプリの実行ファイルあるフォルダにログ(Logs フォルダ)が生成されます。

## 今後の予定

- 機能的にはもういいかなという感じです。
- バックアップ設定を GUI化 したいですね。
