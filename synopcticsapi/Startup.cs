using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

// Questo attributo è CRUCIALE per caricare la classe Startup
[assembly: OwinStartup(typeof(synopcticsapi.Startup))]

namespace synopcticsapi {
    public class Startup {
        public void Configuration(IAppBuilder app)
        {
            // Configura SignalR direttamente qui
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,
                EnableJavaScriptProxies = true
            };

            // Mappa gli hub SignalR (IMPORTANTE, questo genera l'endpoint signalr/hubs)
            app.MapSignalR("/signalr", hubConfiguration);

            // Configura altri servizi se necessario
            // WebSocketService.Configure(app); 
        }
    }
}