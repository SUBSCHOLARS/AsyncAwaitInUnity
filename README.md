# 概要
MicrosoftのC#公式ドキュメントの中にある「非同期処理」をUnityに落とし込みつつ、同期処理との比較を試みたプロジェクトです。
同期処理としてはCoroutine、非同期処理としてはUniTaskを採用しています。これはUIによる視覚化を行なった際に、
通常のTask.Delayでは限界があったためです。
# 使用バージョン
Unity 6000.3.6f1
# 動画
同期処理（Coroutine）

https://youtu.be/iFvv1G-GNbE

非同期処理（UniTask）

https://youtu.be/6hkVoY6hxn8