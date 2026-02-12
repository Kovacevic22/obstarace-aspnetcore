export const LoadingScreen = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-dark">
            <div className="relative">
                <div className="absolute inset-0 rounded-full bg-accent opacity-20 animate-ping"></div>
                <div className="w-16 h-16 border-4 border-accent/10 border-t-accent rounded-full animate-spin"></div>
            </div>

            <div className="mt-8 flex flex-col items-center gap-2">
                <div className="text-white font-black uppercase tracking-[0.5em] text-[10px] animate-pulse">
                    Loading <span className="text-accent">Arena</span>
                </div>
                <div className="w-32 h-px bg-white/5 relative overflow-hidden">
                    <div className="absolute inset-0 bg-accent w-1/2 animate-[loading-bar_1.5s_infinite_ease-in-out]"></div>
                </div>
            </div>
        </div>
    );
};