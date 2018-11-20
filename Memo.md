# 基本的な問題

* 雪同士がくっつく状況を考えると、ある程度の力でぶつける必要があるため、接触だけでくっつくことは考えられない。そこの処理がまだ考えられない。

* くっついているときの状態があまり想像できない。基本的にオブジェクトは球体であることを想像しているが、くっついているときはそれぞれがすこし埋まっているべきだと感じる。  

→表示する半径と実際に接触判定をおこす半径を変える。接触判定をする半径を表示される半径よりも小さくすることで、お互いがのめりこむように見える。

## 雪の表面にポリゴンを張る(ボツ)

1. 雪玉の半径をrとし、２つの雪玉の中心が2r以下の距離の時くっついていると仮定する。
2. くっついているかを無向グラフで示す。基準球の中心Oから2r以内の雪玉を探す。ここで目的球の中心Pが基準球Oから2r以内かどうかを判別するには、基準球の位置を(xo, yo, zo)、目的球の位置を(xp, yp, zp)とすると r^2<(xp-xo)^2+(yp-yo)^2+(zp-zo)^2 により即座に求められる。
3. これによって任意の点にくっついている点をすべて求める。

## 雪の表面にポリゴンを張るその1(ボツ)

1. 雪玉の半径をrとし、２つの雪玉の中心が2r以下の距離の時くっついていると仮定する。
2. 背後に一辺r/2の格子を張る。この格子一つ一つに対して雪玉が触れていればフラグを立てる。
3. 雪玉を全て削除したのち、フラグが立っている格子を白で塗る。

## 雪の表面にポリゴンを張るその3(ボツ)

1. 直径が雪玉の1/8程度の雪玉を用意する。この雪玉は、1.rigidbodyがアタッチされていない, 2.ColliderはSphereColliderのみ 3.Scriptは専用のものである。 の条件を満たす。これを以後サーフェス雪玉と称す。
2. すべての雪玉において、y座標が一番大きな雪玉の座標と一番小さな雪玉の座標を保存しておく。
3. 一つ一つの雪玉に対してその表面をすべて覆うようにサーフェス雪玉を配置していく。サーフェス雪玉はrigidbodyがアタッチされていないため、通常の雪玉よりも処理が軽い。
4. 配置完了後、通常の雪玉をすべて削除する。
5. サーフェス雪玉のy軸方向の配置を小数点第3位以下が0になるように小数点第3位で四捨五入する。こうすることで、グローバル座標で見た場合、サーフェス雪玉が存在するとしても0.01毎にすることができる。

## 雪の表面にポリゴンを張るその4(ボツ)

* グリッドを用意し、グリッドと雪玉との接触の仕方によって勾配を求める。または接触している雪玉のある点での法線ベクトルを求める。
* 雪玉の中心座標と半径があらかじめ分かっていることから、任意の雪玉のボリュームデータは即座に求められる。あとはそれの重ね合わせをによって、それぞれのグリッドのボリュームデータを更新。ポリゴン生成時は、そのグリッドとそのグリッドが属している雪玉の中心座標によって、そのグリッドの方向ベクトルを求める。

## ボリュームデータの構造候補

* 密度
* 勾配
* 法線ベクトル
* 面の方程式