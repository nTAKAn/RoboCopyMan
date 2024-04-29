# RoboCopyMan
---
## 何をするものか？

- robocopy コマンドを定期的に実行するだけのソフトです。
- 開発コンセプトは、できるだけ シンプルに そして 主張 しないです。

> ふだん Dropbox を作業フォルダに使用しているのですが心配性なので、定期的にローカルの NAS にバックアップをとりたいなと思い
いろいろ試してみました。
> しかしながら、バッチファイルは窓が消せなかったり、タスクスケジューラでやろうにもパスワードレスのMicrosoftアカウントではやっぱり窓が消せなかったり、
その他あるかもしれないのですが、結局よくわからないのでこのソフトを作りました。

> ちなみに、RoboCopyScheduler というフリーソフトがあるのですが、それが私の環境ではうまく使えなかったんですよね・・・
> RoboCopyScheduler がうまく使える方は、RoboCopyScheduler の方が高機能ですのでそちらを使用した方がよいです。

## 注意

- 最低限の機能しか実装していません。ライセンスはフリーなので、勝手にいじってくださいませ・・・
- 不明点はコード見た方が早いかも？
- 本ソフトの使用はもちろん自己責任ですが、事故ると私も心苦しいので・・・まずは消えてもよいファイルで試してください。

## インストール、アンインストール

- ダウンロードを展開したフォルダにある実行ファイル RoboCopyMan.exe を実行すれば OK です。
- レジストリは使用していないので、フォルダごと削除で OK です。

## 使い方

- settings フォルダの中に、バックアップ設定を書き込んだ TOML ファイルを格納しておきます。

- sample.toml が入ってますが、これはサンプルファイルです。
> このファイルをコピーして、任意の名前で保存してください。

- バックアップ設定はバックアップごとに作ります（複数格納可能です）。
> sample.toml は、誤動作防止のため読み込まれませんので、sample.toml 以外の名前である必要があります。

## 設定ファイルの書き方

必須項目は、title、srcDir、dstDir、option とバックアップ間隔設定です。

- title は、好きな名前をつけれます。
- srcDir は、コピー元
> TOML で `\` を記述するには `\\` と記述してください。
- dstDir は、コピー先
- option は、robocopy コマンドのオプションです。
> 例えば・・・`"/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE"` は、NAS にバックアップする際に使用する際の一般的なオプションです。
> 
> 詳細は、robocopy 本家のヘルプを参照してください。

#### ログ設定 (logDir, logFilePrefix, logDatetimeFmt) は、任意です。コメントアウトすることでログ出力を無効化できます。
- logDir： ログの出力先になります。実際のログファイル名は、logFilePrefix に 日付(logDatetimeFmt) を付加したファイル名で保存されます。
- logFilePrefix： ログファイル名のプレフィックスです。
- logDatetimeFmt： ログファイルの日付フォーマットです。C#の日付フォーマットに準拠しています。

#### 除外ファイル設定 (xdFiles) は、任意です。コメントアウトすることで除外ファイルを無効化できます。
- xdFiles は、除外するディレクトリです。
> 例えば・・・ `"\"System Volume Information\" \"$RECYCLE.BIN\""`
> 
> TOML でダブルクォーテーションを記述するには `\"` と記述してください。

#### バックアップ間隔設定
- intervalMinutes は、バックアップの実行間隔で、この時間ごとに robocopy が実行されます。
- delayMinutes は、アプリ起動後初回の実行時間です。
> ネットワークドライブの接続を待つために少し遅れて実行させる用途に使用します

> ちなみにですが、あえて【ログ設定、除外ファイル設定】をコメントアウトして、option に必要な設定（ログ、除外ファイル設定）を自分で書くこともできます。

### 設定ファイル例
```
# -*- coding: utf-8 -*-

# バックアップ設定のタイトル
title = "Backup"

# バックアップ元ディレクトリ
srcDir = "C:\\folder"
# バックアップ先ディレクトリ
dstDir = "D:\\backup"
# robocopy のオプション
option = "/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE"

# ログファイルの出力先ディレクトリ 
# (このオプションは任意： コメントアウトすることで無効化（/LOG オプションを付加しない）
logDir = "D:\\backup\\logs"
# ログファイル名
logFilename = "sample-"
# ログファイル名に付加する日付フォーマット (C#の日付フォーマットに準拠)
logDatetimeFmt = "yyyyMMdd-HHmmdd"

# 除外するファイル・ディレクトリ
# (このオプションは任意： コメントアウトすることで無効化（/XD オプションを付加しない）
xdFiles = "\"System Volume Information\" \"$RECYCLE.BIN\""

# バックアップ間隔（分）
intervalMinutes = 60
# 初回実行を遅らせるディレイ（分）
delayMinutes = 5

# 生成される robocopy のコマンド例
# LogFilePath = {LogDir}\\{logFilename}{logDatetimeFmt}.txt
# robocopy {SrcDir} {DstDir} {Option} /LOG:{LogFilePath} /XD {XdFiles}
```

- 上記設定で実行されるコマンド
```
robocopy C:\folder D:\backup /MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE /LOG:D:\backup\logs\sample-20240427-200427.txt /XD "System Volume Information" "$RECYCLE.BIN"
```

## アプリの動作

- 起動するとタスクトレイにごせん像アイコンが表示されます。これが本体です。
- ごせん像を右クリックでメニュー、ダブクリでバックアップ結果を表示するためのダイアログが表示されます。
- バックアップ結果ダイアログのリスト項目をダブルクリックすると、そのバックアップ設定に設定されているログファイルフォルダを開きます。
- ちなみに、アプリの実行ファイルあるフォルダにアプリケーションのログ(Logs フォルダ)が生成されます。

## 今後の予定

- 機能的にはもういいかなという感じです。
- バックアップ設定を GUI化 したいですね。
