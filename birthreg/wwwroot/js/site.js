// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var selectCountry = document.getElementById("country");
var selectState = document.getElementById("state");
async function GetCountry() {
    
    const response = await fetch("https://countriesnow.space/api/v0.1/countries/states");
    const jsonData = await response.json();
    for (let i = 0; i < jsonData.data.length; i++) {
        const countryName = jsonData.data[i].name
        console.log(countryName);
        var el = document.createElement("option");
        el.textContent = countryName;
        el.value = countryName;
        selectCountry.appendChild(el);
    }
}

async function GetState() {
    var select = document.getElementById("state");
    const response = await fetch("https://countriesnow.space/api/v0.1/countries/states");
    const jsonData = await response.json();
    selectCountry.addEventListener("change", function () {
        selectState.innerHTML = "";
        for (let i = 0; i < jsonData.data.length; i++) {
            const countryName = jsonData.data[i];
            if (countryName.name == selectCountry.value){
                console.log(countryName.states);
                for (let i = 0; i < countryName.states.length; i++) {
                    const stateName = countryName.states[i].name;
                    console.log(stateName);
                    var el = document.createElement("option");
                    el.textContent = stateName;
                    el.value = stateName;
                    selectState.appendChild(el);
                }
            }
        }
    });
   
    
}

GetState();
GetCountry();