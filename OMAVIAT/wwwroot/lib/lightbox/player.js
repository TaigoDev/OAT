let player;  
let lastSave;
let IsPlaying;
let CurrentTime;
let SourcesUpdate = false; 
let Links;
let plyr;
function Init() {
    player = new Plyr('#player', {
        i18n: {
            restart: 'Перезапустить',
            rewind: 'Перемотать {seektime}сек',
            play: 'Продолжить',
            pause: 'Пауза',
            fastForward: 'Перемотать {seektime}сек',
            seek: 'Найти',
            seekLabel: '{currentTime} из {duration}',
            played: 'Играет',
            buffered: 'Буферизованный',
            currentTime: 'Текущее время',
            duration: 'Продолжительность',
            volume: 'Звук',
            mute: 'Выкл. звук',
            unmute: 'Вкл. звук',
            enableCaptions: 'Enable captions',
            disableCaptions: 'Disable captions',
            download: 'Скачать',
            enterFullscreen: 'Открыть полноэкранный режим',
            exitFullscreen: 'Закрыть полноэкранный режим',
            frameTitle: 'Плеер для {title}',
            captions: 'Captions',
            settings: 'Настройки',
            pip: 'PIP',
            menuBack: 'Вернуться в предыдущее меню',
            speed: 'Скорость',
            normal: 'Нормальная',
            quality: 'Качество',
            loop: 'Loop',
            start: 'Начать',
            end: 'Закончить',
            all: 'Все',
            reset: 'Сбросить',
            disabled: 'Выключено',
            enabled: 'Включено',
            advertisement: 'Ad',
            qualityBadge: {
                2160: '4K',
                1440: 'HD',
                1080: 'HD',
                720: 'HD',
                576: 'SD',
                480: 'SD',
            },
        },
    });
}
function Play(resolutions){
    IsPlaying = false;
    let sources = [];
    for (const [key, value] of Object.entries(resolutions.links) )
    {
        sources.push({
            src: value.src.replace(":hls:manifest.m3u8", ""),
            type: 'video/mp4',
            size: key
        });
    }
    player.source = 
    {
        type: 'video',
        sources: sources,
        storage: {
            enabled: true,
                key: 'plyr'
        },
    };

    lastSave = Date.now();
    
    player.on('timeupdate', async (event) => {
        plyr = event.detail.plyr;
        if(Date.now() - lastSave > 20000)
        {
            lastSave = Date.now();
            await window.GreetingHelpers.OnSaveAsync(event.detail.plyr.currentTime, event.detail.plyr.duration);
        }
    });
    
    player.on('pause', async (event) =>{
        plyr = event.detail.plyr;
        await window.GreetingHelpers.OnSaveAsync(event.detail.plyr.currentTime, event.detail.plyr.duration);
    });
    player.on('playing', async (event) =>{
        plyr = event.detail.plyr;

        if(!IsPlaying)
        {
            IsPlaying = true;
            if(CurrentTime !== undefined && CurrentTime !== null)
                event.detail.plyr.currentTime = CurrentTime;
            CurrentTime = undefined;
        }
    });
    
    
    plyr = player;

}

function SetCurrentTime(currentTime){
    CurrentTime = currentTime;
    IsPlaying = false;
}

function UpdateSource(links){
    let newSources = [];
    for (const [key, value] of Object.entries(links.links) )
    {
        newSources.push({
            src: value.src.replace(":hls:manifest.m3u8", ""),
            type: 'video/mp4',
            size: key
        });
    }
    plyr.source =  {
        type: 'video',
        sources: newSources,
        storage: {
            enabled: true,
            key: 'plyr'
        },
    };
}

class GreetingHelpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        GreetingHelpers.dotNetHelper = value;
    }

    static async OnSaveAsync(currentTime, duration) {
        await GreetingHelpers.dotNetHelper.invokeMethodAsync('OnSaveAsync', currentTime, duration);
    }
    
}
window.GreetingHelpers = GreetingHelpers;