var M = [];

M[0] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[1] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[2] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[3] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[4] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[5] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[6] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[7] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[8] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
M[9] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
var rox = [];
var roy = [];

function clear() { //done
  for (let i = 0; i < 10; i++) {
    for (let j = 0; j < 10; j++) {
      M[i][j] = 0;
      document.getElementById("d" + i.toString() + j.toString()).style.backgroundColor = "rgb(55, 136, 211)";
    }
  }
  let kekw = document.getElementById("solved");
  kekw.style.opacity = 0;
  return 0;
}

async function reqelems() { //done
  let kek = document.getElementById("solved");
  kek.style.opacity = 0;
  clear();
  let text = "api/update";
  const request = new Request(text);
  const URL = request.url;
  const credentials = request.credentials;
  await fetch(request).then(async response => {
    if (response.status === 200) {
      let d = await response.json();
      for (let i = 0; i < 10; i++) {
        rox[i] = d.rowx[i];
      }
      for (let i = 0; i < 10; i++) {
        roy[i] = parseInt(d.rowy[i]);
      }
      for (let i = 0; i < 10; i++) {
        let keku = document.getElementById("u" + i.toString());
        keku.textContent = d.rowx[i].toString();
      }
      for (let i = 0; i < 10; i++) {
        let kekd = document.getElementById("d" + (i).toString());
        kekd.textContent = d.rowy[i].toString();
      }
      return d;
    }
    else {
      throw new Error('Что-то пошло не так на API сервере.');
    }
  }).then(response => {
    // ...
  }).catch(error => {
    console.error(error);
  });
}

function cd(elem, a, b) { //done
  if (M[a][b] == 1) {
    document.getElementById(elem).style.backgroundColor = "rgb(55, 136, 211)";
    M[a][b] = 0;
  }
  else {
    document.getElementById(elem).style.backgroundColor = "rgb(255, 255, 255)";
    M[a][b] = 1;
  }
}

async function chk() { //on work
  let solved = true;
  for (let i = 0; i < 10; i++) { //M[I][J], слева направо
    let counter = 0;
    for (let j = 0; j < 10; j++) {
      if (M[i][j] == 1) counter++;
    }
    if (counter != roy[i]) solved = false;
  }
  for (let i = 0; i < 10; i++) { //M[J][I], сверху вниз
    let counter = 0;
    for (let j = 0; j < 10; j++) {
      if (M[j][i] == 1) counter++;
    }
    if (counter != rox[i]) solved = false;
  }
for(let i = 0; i<10; i++){
  for (let j = 0; j<10; j++){
    if(M[i][j] == 1){
      try{
       if(!((M[i][j+1] == 1 && M[i+1][j] == 1) ||
       (M[i][j+1] == 1 && M[i-1][j] == 1) ||
       (M[i][j-1] == 1 && M[i-1][j] == 1) ||
       (M[i][j-1] == 1 && M[i+1][j] == 1))) solved = false;
      }
      catch{

      }
    }
  }
}



  let kek = document.getElementById("solved");
  if (solved) {
    kek.style.color = "rgb(55, 136, 211)"
    // kek.style.left = "33%";
    kek.textContent = "РЕШЕНО ВЕРНО"
    kek.style.opacity = 1;
  }
  else {
    kek.style.color = "red"
    // kek.style.left = "40%";
    kek.textContent = "НЕВЕРНО"
    kek.style.opacity = 1;
  }
  return solved;
}