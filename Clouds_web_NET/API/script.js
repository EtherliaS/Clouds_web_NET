
var M = [];

M[0] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[1] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[2] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[3] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[4] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[5] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[6] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[7] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[8] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
M[9] = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ]; 
const rowx = new Array(10);
const rowy = new Array(10);

function update(){
  
}

function req(reqtext){
  const request = new Request(reqtext);
  const URL = request.url;
  const credentials = request.credentials;
  fetch(request).then(response => {
    if (response.status === 200) {
      return response.json();
      console.log('Request succesful');
    }
    else {
      throw new Error('Что-то пошло не так на API сервере.');
    }
  }).then(response => {
    console.debug(response);
    // ...
  }).catch(error => {
    console.error(error);
  });
}



function cd(elem, a, b) {
  if (M[a][b] == 1) {
    document.getElementById(elem).style.backgroundColor = "rgb(55, 136, 211)";
    M[a][b] = 0;
  }
  else {
    document.getElementById(elem).style.backgroundColor = "rgb(255, 255, 255)";
    M[a][b] = 1;
  }
}

function send(){
  var sl = "api/check/"
  for (let i = 0; i<10; i++){
    for (let j = 0; j<10; j++){
      sl = sl + M[i][j].toString();
    }
  }
  resp = req(sl);
}