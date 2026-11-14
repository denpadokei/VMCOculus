# VMCOculus

[![Build and Release](https://github.com/denpadokei/VMCOculus/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/VMCOculus/actions/workflows/build-and-release.yml)

Oculus 版 Beat Saber の HMD トラッキングを OSC で [バーチャルモーションキャプチャー](https://github.com/sh-akira/VirtualMotionCapture) へ送る MOD です。Oculus 版で VMC の頭が固まってしまう問題を解決します。

![vmpc_logo_128x128](https://user-images.githubusercontent.com/55026301/99907665-6c5ae900-2d21-11eb-81f0-b91c8d1d5da9.png)

## 機能

- Beat Saber の HMD の位置と回転を OSC で送信
- 送信先は `127.0.0.1:39540`、約 10ms 間隔で送信
- アドレスは `/VMC/Ext/Hmd/Pos`、引数はシリアル名 `BeatSaber Virtual HMD` と位置 (x, y, z)、回転 (x, y, z, w)
- VMC 側ではこのシリアル名のトラッカーが HMD として扱われる

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.13.0 |
| 依存 MOD | BSIPA 4.1.3 |

## インストール

1. Beat Saber を 1.13.0 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/VMCOculus/releases) から `VMCOculus-0.0.2-bs1.13.0.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

VMC 側で OSC 受信を有効にしておいてください。

1. VMC を起動する
2. Beat Saber を起動する
3. VMC の詳細設定 > トラッカーの割り当て設定を開き、HMD を `BeatSaber Virtual HMD` に変更する
4. キャリブレーションする

## ビルド

```powershell
git clone https://github.com/denpadokei/VMCOculus.git
cd VMCOculus
dotnet build .\VMCOculus\VMCOculus.csproj -c Release
```

`BeatSaberDir` に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT

本 MOD は [uOSC](https://github.com/hecomi/uOSC) を使用しています。

The MIT License (MIT)

Copyright (c) 2017 hecomi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
