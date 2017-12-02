/// <reference path="../typings/jquery/jquery.d.ts" />

interface IRace {
    Name: string;
}


interface IRank {
    Name: string;
}

interface IArchetype {
    Name: string;
}

//Calling REST endpoints
//https://visualstudiomagazine.com/articles/2013/10/01/calling-web-services-with-typescript.aspx


function readValue(controlName: string): string {
    var control = <HTMLSelectElement>$('#' + controlName)[0];
    return control.value;
}

function readValueInt(controlName: string): number {
    var control = <HTMLSelectElement>$('#' + controlName)[0];
    return parseInt(control.value);
}

function readChecked(controlName: string): boolean {
    var control = <HTMLInputElement>$('#' + controlName)[0];
    return control.checked;
}

function settingChanged(setting: string, archetype: HTMLSelectElement, race: HTMLSelectElement, rank: HTMLSelectElement) {

    $(archetype).empty();
    $(race).empty();
    $(rank).empty();


    $.getJSON("/SettingApi/Archetypes?setting=" + encodeURIComponent(setting),
        cs => {

            var myList = <IArchetype[]>cs;

            archetype.appendChild(new Option("", ""));

            for (var i = 0; i < myList.length; i++) {
                var opt = new Option(myList[i].Name, myList[i].Name);
                archetype.appendChild(opt);
            }
        });

    $.getJSON("/SettingApi/Races?setting=" + encodeURIComponent(setting),
        cs => {

            var myList = <IRace[]>cs;

            race.appendChild(new Option("", ""));

            for (var i = 0; i < myList.length; i++) {
                var opt = new Option(myList[i].Name, myList[i].Name);
                race.appendChild(opt);
            }
        });

    $.getJSON("/SettingApi/Ranks?setting=" + encodeURIComponent(setting),
        cs => {

            var myList = <IRank[]>cs;

            rank.appendChild(new Option("", ""));

            for (var i = 0; i < myList.length; i++) {
                var opt = new Option(myList[i].Name, myList[i].Name);
                rank.appendChild(opt);
            }
        });
}

function generateSquad(setting: string, archetype: string, race: string, rank: string, squadCount: number) {
    window.location.href = "/Home/Squad?setting=" + encodeURIComponent(setting) + "&archetype=" + encodeURIComponent(archetype) + "&race=" + encodeURIComponent(race) + "&rank=" + encodeURIComponent(rank) + "&squadCount=" + squadCount;
}

function generateRiftsMission(pace: number, eventFrequency: number, type: string)
{
    window.location.href = "/Home/RiftsMission?pace=" + pace + "&eventFrequency=" + eventFrequency + "&type=" + type;
}

