const PostFunc = async (url = '', data = {}) => {
    const responce = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
       // body: JSON.stringify(data)
    });
    return responce.json();
}

const GetFunc = async (url = '', data = {}) => {
    const responce = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    });
    return responce.json();
}

GetFunc('https://localhost:44310/api/Coins/GetAllCoins').then((data => {
    data.forEach((el) => {

    });
}))

PostFunc('https://localhost:44310/api/Authorization/AuthUser?login=adm&password=adm')
.then((data => {
    console.log(data);
}))