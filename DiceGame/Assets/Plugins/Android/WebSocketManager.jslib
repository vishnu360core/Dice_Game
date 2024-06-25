
mergeInto(LibraryManager.library, {
  WebSocketInit: function(url) 
  {
    var urlString = Pointer_stringify(url);

    this.socket = new WebSocket(urlString);

     window.websockets.push(this.socket);
     console.log("Websockets :",window.websockets);

     var count = window.websockets.length - 1 ;

    this.socket.onopen = function() {
      console.log("Connected"); 

       window.unityInstance.SendMessage('Network', 'ConnectCallBack', count);
    };

    this.socket.onerror = function(error) 
    {
      console.log("Error :",error); // Pass -1 to indicate failure
    };

    this.socket.onmessage = function(event) 
    {
      console.log('Data >>>>>>>>>>>>>>>>>>>>>>>>>>', event.data)

        window.unityInstance.SendMessage('Network', 'ReceiveMessage', event.data + '@' + count);
    }
  },
  Send: function(index,message) 
  { 
    console.log("index",index);
     
    console.log("Web Send message",Pointer_stringify(message));
    window.websockets[index].send(Pointer_stringify(message));
  }
});

