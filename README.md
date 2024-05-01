# RoboCopyMan
---
## 何をするものか？

- robocopy コマンドを定期的に実行するだけのソフトです。
- 開発コンセプトは、できるだけ シンプルに、そして 主張 せず、あとくされなく です。

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
> フォームを表示しない常駐型ですので、起動後はタスクトレイに格納されます。

- いらなくなったときは、レジストリは使用していないので、フォルダごと削除で OK です。

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

#### 除外ファイル設定 (xdDirs, xfFiles) は、任意です。コメントアウトすることで除外ファイルを無効化できます。
- xdDirs は、除外するディレクトリ、xfFiles は除外するファイルです。
> 例えば・・・ `"\"System Volume Information\" \"$RECYCLE.BIN\""`
> 
> TOML でダブルクォーテーションを記述するには `\"` と記述してください。
> 空の文字列でもエラーになりませんが、除外するディレクトリ、ファイルがない場合はコメントアウトしてください。

#### テストモード (testMode) は、任意です。コメントアウトすることでテストモードを無効化できます。
- testMode は、robocopy の /L オプションを付加するかどうかです。

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
# 表示順番 (このオプションは任意: order を指定すると order で降順ソートして表示します)
order = 1

# バックアップ元ディレクトリ
srcDir = "C:\\folder"
# バックアップ先ディレクトリ
dstDir = "D:\\backup"
# robocopy のオプション
option = "/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE"

# ログファイルの出力先ディレクトリ (このオプションは任意： コメントアウトすることで無効化（/LOG オプションを付加しない）
# logDir = "D:\\backup\\logs"
# ログファイル名のプレフィックス
# logFilePrefix = "sample-"
# ログファイル名のプレフィックスに付加する日付フォーマット (C#の日付フォーマットに準拠)
# logDatetimeFmt = "yyyyMMdd-HHmmss"

# 除外するディレクトリ・ファイル (このオプションは任意： コメントアウトすることで無効化（/XD, /XF オプションを付加しない）
xdDirs = "\"System Volume Information\" \"$RECYCLE.BIN\""
# xfFiles = ""
# テストモード (true: テストモード /L オプションを付加する, false または コメントアウト: 実際にコピーする)
testMode = true

# 強制バックアップ無効設定 (true: 強制バックアップ無効, false または コメントアウト: 強制バックアップ有効)
# disableForcedBackup = true

# バックアップ間隔（分）
intervalMinutes = 60
# 初回実行を遅らせるディレイ（分）
delayMinutes = 5

# sample.toml で生成される robocopy コマンド例
#     LogFilePath = {LogDir}\\{logFilename}{logDatetimeFmt}.txt
#     robocopy {SrcDir} {DstDir} {Option} /LOG:{LogFilePath} /XD {xdDirs} /XF {xfFiles}
#
# robocopy C:\folder D:\backup /L /MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE /LOG:D:\backup\logs\sample-20210101-123456.txt /XD "System Volume Information" "$RECYCLE.BIN"

```

## アプリの動作

- 起動するとタスクトレイにごせん像アイコンが表示されます。これが本体です。
- ごせん像を右クリックでメニュー、ダブクリでバックアップ結果を表示するためのダイアログが表示されます。
- バックアップ結果ダイアログのリスト項目をダブルクリックすると、そのバックアップ設定に設定されているログファイルフォルダを開きます。
- ちなみに、アプリの実行ファイルあるフォルダにアプリケーションのログ(Logs フォルダ)が生成されます。

## 今後の予定

- 機能的にはもういいかなという感じです。
- バックアップ設定を GUI化 したいですね。
