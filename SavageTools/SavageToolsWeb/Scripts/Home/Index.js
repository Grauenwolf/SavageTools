/// <reference path="../typings/jquery/jquery.d.ts" />
//Calling REST endpoints
//https://visualstudiomagazine.com/articles/2013/10/01/calling-web-services-with-typescript.aspx
function readValue(controlName) {
    var control = $('#' + controlName)[0];
    return control.value;
}
function readValueInt(controlName) {
    var control = $('#' + controlName)[0];
    return parseInt(control.value);
}
function readChecked(controlName) {
    var control = $('#' + controlName)[0];
    return control.checked;
}
function settingChanged(setting, archetype, race, rank) {
    $(archetype).empty();
    $(race).empty();
    $(rank).empty();
    $.getJSON("/SettingApi/Archetypes?setting=" + encodeURIComponent(setting), function (cs) {
        var myList = cs;
        archetype.appendChild(new Option("", ""));
        for (var i = 0; i < myList.length; i++) {
            var opt = new Option(myList[i].Name, myList[i].Name);
            archetype.appendChild(opt);
        }
    });
    $.getJSON("/SettingApi/Races?setting=" + encodeURIComponent(setting), function (cs) {
        var myList = cs;
        race.appendChild(new Option("", ""));
        for (var i = 0; i < myList.length; i++) {
            var opt = new Option(myList[i].Name, myList[i].Name);
            race.appendChild(opt);
        }
    });
    $.getJSON("/SettingApi/Ranks?setting=" + encodeURIComponent(setting), function (cs) {
        var myList = cs;
        rank.appendChild(new Option("", ""));
        for (var i = 0; i < myList.length; i++) {
            var opt = new Option(myList[i].Name, myList[i].Name);
            rank.appendChild(opt);
        }
    });
}
function generateSquad(setting, archetype, race, rank, squadCount) {
    window.location.href = "/Home/Squad?setting=" + encodeURIComponent(setting) + "&archetype=" + encodeURIComponent(archetype) + "&race=" + encodeURIComponent(race) + "&rank=" + encodeURIComponent(rank) + "&squadCount=" + squadCount;
}
function generateRiftsMission(pace, eventFrequency) {
    window.location.href = "/Home/RiftsMission?pace=" + pace + "&eventFrequency=" + eventFrequency;
}
//# sourceMappingURL=Index.js.map