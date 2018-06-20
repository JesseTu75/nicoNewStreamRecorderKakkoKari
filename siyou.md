
## 製作中に見つけたニコ生新配信仕様

誰かの役に立てば幸いです。
独自の経験のため、間違っている部分もあるかもしれません。

### 視聴ページ
- たまに500 internal server errorが返ります。
- 放送終了直後は視聴ページでも放送後の表示のページでもない、非ログインのときに表示されるページ？のようなところに飛ばされることがあります。まだ放送終了後ページが用意されていないのかもしれません。

### websocket
接続時
- まず基本メッセージは{"type":"watch","body":{"command":"getpermit","requirement":{"broadcastId":"6121945301627","route":"","stream":{"protocol":"hls","requireNewStream":true,"priorStreamQuality":"normal","isLowLatency":true},"room":{"isCommentable":true,"protocol":"webSocket"}}}}
- このうちのrequireNewStreamは名前の通り新しい映像ストリームを要請する意味だと思われます。ここにfalseを指定することで既存の映像ストリームを切断することなく既存の映像ストリームを取得することができます。ブラウザでは最初にプレイヤーをロードしたときにはtrueで接続され、プレイヤー右下の更新ボタンを押すとfalseでメッセージが送られるようです。これを利用してツール側でfalseで接続すれば、ある程度はブラウザ視聴に干渉せずに接続することができますが、有料会員限定放送の非会員でも視聴できる部分？などは例外としてfalseで接続しても既存の接続を切断して新規ストリームが返ってくるようです。完全な確認は取れていませんが、どうやらサーバーから送られてきたcurrentstreamのmediaServerAuthがnullでない場合はfalseが効かないような気がします。既存の有効なストリームが存在しない状態でfalseで接続disconnectされて「NO_PERMISSION」が返ってきます。本ツールでは一旦falseで送ってNO_PERMISSIONが返ってきたときだけtrueで接続し直すという手法を取っていますが、映像クオリティ選択機能との兼ね合いで機能しないことも多いです。
- priorStreamQualityは新規に取得したい映像ストリームのクオリティを指定します。ほとんどの放送では自動はabr、3Mbpsはsuper_high、2Mbpsはhigh、1Mbpsはnormal、384kbpsはlow、192kbpsはsuper_lowに対応しており、一部の放送の高画質はhigh、低画質はnormalに対応しています。また、有料放送などでは高画質＝highが会員映像、normalが非会員映像に分けられていることもありますがそうでないこともあります。この項目の設定はrequireNewStreamにtrueを指定した場合にのみ有効になります。多くの放送ではsuper_highが存在しない放送にsuper_highを指定したり「abc」などの適当な文字列を指定した場合は前回有効な接続をしたときの設定でストリームが返ってくるようですが、高画質と低画質しか画質選択が存在しない放送でrequireNewStreamがfalseだったとしても、highとnormal以外を指定すると有効なストリームの取得ができなかったことがありました。いきなり目的の画質を指定するとNO_PERMISSIONになる可能性があるので、一旦何も指定せずに投げて、返ってきたcurrentStreamのqualityTypesから目的の画質を指定して送り直すのも手かもしれません。

サーバーからのcurrentStream
- mediaServerTypeはほとんどがdmcですが、もう一つの種類があったと思います。が、手元にログがないので定かではないです。

statistics
- 極希に「{"type":"watch","body":{"command":"statistics","params":["2",null,"0","0"]}}」のようにnullが入っていることがあります。

disconnect
- disconnectを受け取った後にgetpermitで再接続する際は、requireNewStreamをfalseにしていると前回の切断理由が返ってきて再びdisconnectになってしまうようです。disconnectされた後は一度requireNewStreamをtrueにする必要があるようです。
- NO_PERMISSION: 既存の有効なストリームがない状態でrequireNewStreamをfalseに指定したりなど、getpermitが成功しなかったときに送られてくるようです。
- TAKEOVER: いわゆる追い出しです。rtmp配信と違い、新配信では追い出し時にwebsocketの接続自体が切られてしまいます。
- SERVICE_TEMPORARILY_UNAVAILABLE: ある放送へのアクセス過多？大量のスレッドで集中的にアクセスすると起こりました。放送の最初でも起こりやすい？ただ、それほど人がいない放送の予約枠の最初でも起こっていました。ユーザー側の環境により放送の最初に頻発することもあるようです。
- END_PROGRAM: 番組終了
- TOO_MANY_CONNECTIONS: あるアカウントあたりのある放送への接続が多すぎる。10ぐらい？一つのaudience_tokenごとではなく、アカウントごとの気がします。つまり、一つのアカウントを使って大量の別放送を録画する分には引っかからない、かも。また、逆を言えば一つのアカウントから制限数以下の複数のストリームの取得が可能なので、使い方はありそうです。ただし、一つのアカウントに対して配信されるストリームは一つのクオリティのみのようです。
- INTERNAL_SERVERERROR: こちらもあまりよく理解できていませんが、稀に送られてきます。
