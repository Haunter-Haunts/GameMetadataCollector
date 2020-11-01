var steamGameInput = document.getElementById('steamGameInput');
steamGameInput.addEventListener('input', steamGameInputEvent);

function steamGameInputEvent(e) {
    searchForSteamGame(e.target.value);
    // console.log(e.target.value);
}