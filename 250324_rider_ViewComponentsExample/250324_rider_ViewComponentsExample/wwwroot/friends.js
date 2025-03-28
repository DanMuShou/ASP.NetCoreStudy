document.
    querySelector("#load-friends-button").
    addEventListener(
        "click", 
        async function () {
            var response =  await fetch("friendList",{method:"GET"})
            var responseBody = await response.text();
            document.querySelector("#list").innerHTML = responseBody;
            });