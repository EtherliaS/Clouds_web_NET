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

async function reqelems() { //done
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

async function send() { //on work
  let sand = JSON.stringify(M); //pesok
  var sl = "api/check/"
  for (let i = 0; i < 10; i++) {
    for (let j = 0; j < 10; j++) {
      sl = sl + M[i][j].toString();
    }
  }
  // sl = sl + ",";
  sl = sl + rox.join('');
  // sl = sl + ",";
  sl = sl + roy.join('');
  const request = new Request(sl);
  const URL = request.url;
  const credentials = request.credentials;
  await fetch(request).then(async response => {
    if (response.status === 200) {
      let d = await response.json();
      if (d.solved) {
        console.log("true");
        return true;
      }
      else {
        console.log("not true");
        return false;
      }
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