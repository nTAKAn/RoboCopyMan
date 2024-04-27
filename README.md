# RoboCopyMan

## 何をするものか？

robocopy コマンドを定期的に実行するだけのソフトです。
でも案外このシンプルさを持つソフトはないので自分では重宝しています。

## 注意

- 最低限の機能しか実装していません。ライセンスはフリーなので、勝手にいじってくださいませ・・・
- 不明点はコード見た方が早いかも？

## 使い方

settings フォルダの中に、バックアップ情報を書き込んだ toml ファイルを格納しておきます。

x-drive.toml が入ってますが、これは私の設定ファイルですので参考にして製作してください。
変えるポイントは、title、srcDir、dstDir、logDir でよいです。

- title は、好きな名前をつけれます。
- srcDir は、コピー元
- dstDir は、コピー先
- logDir は、ログの出力先になります。実際のログファイル名は、dstDir に 日付 をつけたファイル名で保存されます。
（MainPC-XDrive-20240427-190427.txt のような感じです。）
- intervalMinutes は、バックアップの実行間隔で、この時間ごとに robocopy 実行されます。
- delayMinutes は、アプリ起動後初回の実行時間です。アプリ起動直後に robocopy が実行されるとまずい場合
（例えば、ネットワークドライブの接続を待ってなど）は適当に設定します。
- option は、何も設定しないと "/MIR /XJF /XJD /COPY:DAT /DCOPY:DAT /FFT /R:1 /W:10 /MT:128 /NP /TEE" が設定されます。
（この機能はテストしてません）
- exclude は、何も設定しないと "\"System Volume Information\" \"$RECYCLE.BIN\"" が設定されます。
（この機能はテストしてません）

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

# バックアップ間隔（分）
intervalMinutes = 60
# 初回実行を遅らせるディレイ（分）
delayMinutes = 5

# robocopy のオプション
# option =

# 除外するファイル・ディレクトリ
# exclude =
```
