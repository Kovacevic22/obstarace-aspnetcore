export function OfflinePage() {
    return (
        <div className="min-h-screen bg-dark flex flex-col items-center justify-center p-6 text-center">
            <div className="space-y-4">
                <h1 className="text-4xl font-black text-white uppercase italic tracking-tighter">
                    [ SYSTEM_OFFLINE ]
                </h1>
                <div className="w-16 h-1 bg-accent mx-auto animate-pulse"></div>
                <p className="text-white/40 text-[10px] uppercase tracking-[0.3em] font-bold">
                    OFFLINE_MODE_ACTIVE <br />
                    SERVER_RESPONSE_TIMEOUT.
                </p>
                <button
                    onClick={() => window.location.href = "/"}
                    className="mt-8 border border-white/10 px-6 py-2 text-[10px] text-white/60 hover:text-accent transition-all uppercase font-black cursor-pointer"
                >
                    Retry Connection
                </button>
            </div>
        </div>
    );
}
export default OfflinePage;