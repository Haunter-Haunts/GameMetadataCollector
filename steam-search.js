function searchForSteamGame(val) {
    var search = 'https://store.steampowered.com/search/results?term=' + encodeURI(val) + '&force_infinite=1&snr=1_7_7_151_7'

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            parseSearchData(this.responseText);
        }
    };
    xhttp.open("GET", search, true);
    xhttp.send();
}

function parseSearchData(data) {
    var element = document.createElement('div');
    element.innerHTML = data;
    var anchors = element.getElementsByTagName('a');
    for (var x = 0; x < anchors.length; x++) {
        var result = anchors[x];
        var appId = result.getAttribute("data-ds-appid");
        if (appId !== undefined && appId !== null && appId !== '') {
            var appImg = getChildByClass(result, 'search_capsule').childNodes[0].getAttribute('src');
            var searchNameCombined = getChildByClass(result, 'responsive_search_name_combined');
            var searchName = getChildByClass(searchNameCombined, 'search_name');
            var appTitle = getChildByClass(searchName, 'title').textContent;
            var obj = {
                AppId: appId,
                AppImg: appImg,
                AppTitle: appTitle
            };
            console.log(obj);
        }
    }
}

function getChildByClass(parent, className) {
    for (var i = 0; i < parent.childNodes.length; i++) {
        if (parent.childNodes[i].className !== null 
            && parent.childNodes[i].className !== undefined
            && parent.childNodes[i].className !== ''
            && parent.childNodes[i].className.includes(className)) {
            return parent.childNodes[i];
        }
    }
}