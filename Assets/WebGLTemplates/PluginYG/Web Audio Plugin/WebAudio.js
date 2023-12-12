const audioPath = "./StreamingAssets/Audio/";
const sourceDictionary = {};

let onSourceEndCallback = undefined;

function InitAudio(sourceEndCallback) {
    onSourceEndCallback = sourceEndCallback;
}

function PlayAudioSource(sourceID, clipPath, loop, volume, mute, pitch, time){
    if(sourceDictionary[sourceID] !== undefined){
        sourceDictionary[sourceID].stop();
    }

    clipPath = audioPath + clipPath;

    var source = new Howl({
        src: [clipPath],
        html5: false,
        loop: Boolean(loop),
        autoplay: true,
        volume: volume,
        mute: Boolean(mute),
        rate: pitch,
    })

    source.seek(time);
    source.on("end", function(){
        onSourceEndCallback(sourceID);
    });

    sourceDictionary[sourceID] = source;
}

function SetAudioSourceLoop(sourceID, value) {
    const source = sourceDictionary[sourceID];

    if(source !== undefined){
        source.loop(Boolean(value));
    }
}

function SetAudioSourceVolume(sourceID, value) {
    const source = sourceDictionary[sourceID];

    if(source !== undefined){
        source.volume(value);
        console.log("set volume");
    }
}

function SetAudioSourceMute(sourceID, value) {
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        source.mute(Boolean(value));
    }
}

function StopAudioSource(sourceID) {
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        source.stop();
        console.log("stop");
    }
}

function DeleteAudioSource(sourceID){
    StopAudioSource(sourceID);

    sourceDictionary[sourceID] = undefined;
}

function SetAudioSourcePitch(sourceID, value){
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        source.rate(value);
    }
}

function GetAudioSourcePitch(sourceID){
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        return source.rate();
    }

    return 0;
}

function SetAudioSourceTime(sourceID, value){
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        source.seek(value);
    }
}

function GetAudioSourceTime(sourceID){
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        return source.seek();
    }

    return 0;
}

function IsPlayingAudioSource(sourceID){
    const source = sourceDictionary[sourceID];
    
    if(source !== undefined){
        return source.playing();
    }

    return false;
}

function PauseAudioSource(sourceID) {
    const source = sourceDictionary[sourceID];

    if(source !== undefined){
        source.pause();
    }
}

function UnpauseAudioSource(sourceID) {
    const source = sourceDictionary[sourceID];

    if(source !== undefined){
        source.play();
    }
}

// Global Audio
function SetGlobalVolume(value){
    Howler.volume(value);
}

function AudioMute(value){
    Howler.mute(Boolean(value));
}

function PauseAudio(){
    AudioMute(true);
}

function UnPauseAudio(){
    AudioMute(false)
}