# SPModelImporter
SubstancePainterから出力したモデル群をUnityでセットアップするスクリプト
SPModelImporter/import ModelからFbxモデルを選択することでマテリアル等が設定済みのモデルがインポートできます。

`現在 : URPにのみ対応`

## Usage

### 1. How to setup for Unity

  UnityのPackageManagerで、Add Package From git url...に次のパスを指定
  `https://github.com/Astript/SPModelImporter.git?path=Assets/SPModelImporter/`
  ProjectSettings>SPModelImporterのタブから詳細な設定を変えることができます。
  
  ※ デフォルトの値
  
  ![image](https://user-images.githubusercontent.com/53074461/140527058-b793667e-520f-474e-ae13-6ea5ef91a34c.png)

  
### 2. How to Model Export SubstancePainter

  以下の画像に沿って、Unity上で設定したフォルダ名にテクスチャの保存を行う.
  <img width="500px" src="https://user-images.githubusercontent.com/53074461/140524523-dd942b41-e703-49bd-b66b-afbf9895444e.png" />

  フォルダ名にtexを指定した場合

  ![image](https://user-images.githubusercontent.com/53074461/140526338-870c4b2f-313e-4ab9-ac09-cd50ca5ed99e.png)

### 3. Use

SPModelImporter/import ModelからFbxまたはobjのファイルを読み込むことでマテリアルが設定済みのモデルが生成されます。<br>
それだけ
