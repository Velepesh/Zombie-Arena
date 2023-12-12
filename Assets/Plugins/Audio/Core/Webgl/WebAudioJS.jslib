const library = {
    $db: {
        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },

    InitAudio: function(sourceEndCallback){
        window.InitAudio(function(sourceID){
            sourceID = db.allocateUnmanagedString(sourceID);
            dynCall('vi', sourceEndCallback, [sourceID]);
        });
    },

    PlayAudioSource: function (sourceID, clipPath, loop, volume, mute, pitch, time) {
        sourceID = UTF8ToString(sourceID);
        clipPath = UTF8ToString(clipPath);
        
        window.PlayAudioSource(sourceID, clipPath, loop, volume, mute, pitch, time);
    },

    SetSourceLoopExtern: function (sourceID, loop){
        sourceID = UTF8ToString(sourceID);

        window.SetAudioSourceLoop(sourceID, loop);
    },

    SetAudioSourceVolume: function (sourceID, value){
        sourceID = UTF8ToString(sourceID);

        window.SetAudioSourceVolume(sourceID, value);
    },

    SetAudioSourceMute: function  (sourceID, value){
        sourceID = UTF8ToString(sourceID);

        window.SetAudioSourceMute(sourceID, value);
    },

    StopAudioSource: function  (sourceID){
        sourceID = UTF8ToString(sourceID);

        window.StopAudioSource(sourceID);
    },

    MuteExtern: function (value){
        window.AudioMute(value);  
    },

    SetGlobalVolume: function (value){
        window.SetGlobalVolume(value);  
    },

    DeleteAudioSource: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        window.DeleteAudioSource(sourceID);
    },

    SetAudioSourcePitch: function(sourceID, value){
        sourceID = UTF8ToString(sourceID);

        window.SetAudioSourcePitch(sourceID, value);
    },

    GetAudioSourcePitch: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        return window.GetAudioSourcePitch(sourceID);
    },

    SetAudioSourceTime: function(sourceID, value){
        sourceID = UTF8ToString(sourceID);

        window.SetAudioSourceTime(sourceID, value);
    },

    GetAudioSourceTime: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        return window.GetAudioSourceTime(sourceID);
    },

    IsPlayingAudioSource: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        return window.IsPlayingAudioSource(sourceID);
    },

    PauseAudioSource: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        window.PauseAudioSource(sourceID);
    },

    UnpauseAudioSource: function(sourceID){
        sourceID = UTF8ToString(sourceID);

        window.UnpauseAudioSource(sourceID);
    },

    PauseAudio: function(){
        window.PauseAudio();
    },

    UnPauseAudio: function(){
        window.UnPauseAudio();
    },
}


autoAddDeps(library, '$db');
mergeInto(LibraryManager.library, library);