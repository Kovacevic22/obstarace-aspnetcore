export function Footer() {
    return (
        <div className="bg-[var(--color-dark)] text-[var(--color-light)] border-t border-[var(--color-secondary)]/20">
            <div className="container mx-auto px-6 py-10">

                <div className="grid grid-cols-1 md:grid-cols-3 gap-10">

                    <div className="flex flex-col gap-4">
                        <div className="text-2xl font-black uppercase tracking-tighter">
                            Obsta<span className="text-[var(--color-accent)]">Race</span>
                        </div>
                        <div className="text-sm opacity-60 max-w-xs">
                            Central platform for OCR events. Find, register, and dominate the trail.
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="flex flex-col gap-2">
                            <div className="font-bold uppercase text-xs tracking-widest mb-2">Navigation</div>
                            <div className="text-sm opacity-70 hover:text-[var(--color-accent)] cursor-pointer">Calendar</div>
                            <div className="text-sm opacity-70 hover:text-[var(--color-accent)] cursor-pointer">Races</div>
                            <div className="text-sm opacity-70 hover:text-[var(--color-accent)] cursor-pointer">About</div>
                        </div>
                        <div className="flex flex-col gap-2">
                            <div className="font-bold uppercase text-xs tracking-widest mb-2">Legal</div>
                            <div className="text-sm opacity-70 hover:text-[var(--color-accent)] cursor-pointer">Privacy</div>
                            <div className="text-sm opacity-70 hover:text-[var(--color-accent)] cursor-pointer">Terms</div>
                        </div>
                    </div>

                    <div className="flex flex-col gap-6">
                        <div className="font-bold uppercase text-[10px] tracking-[0.3em] text-[var(--color-accent)] opacity-80">
                            Ecosystem Partners
                        </div>
                        <div className="flex flex-wrap gap-x-8 gap-y-4 items-center opacity-40 grayscale hover:grayscale-0 transition-all duration-500">
                            <div className="text-xl font-bold tracking-tighter flex items-center gap-1">
                                GARMIN<span className="w-1 h-1 bg-white rounded-full"></span>
                            </div>
                            <div className="text-xl font-black italic tracking-tight text-[#FC4C02]">
                                STRAVA
                            </div>
                            <div className="text-lg font-light tracking-[0.2em] border border-white/30 px-2 py-0.5">
                                SUUNTO
                            </div>
                            <div className="text-sm font-black uppercase tracking-widest flex flex-col leading-none">
                                <span>Red</span>
                                <span className="text-[var(--color-accent)]">Bull</span>
                            </div>
                        </div>

                        <div className="text-[10px] opacity-40 max-w-[200px] leading-tight">
                            Seamlessly sync your race results with your favorite fitness platforms.
                        </div>
                    </div>

                </div>
                <div className="mt-12 pt-6 border-t border-white/5 flex justify-between items-center text-[10px] opacity-30 uppercase tracking-[0.2em]">
                    <div>© 2025 ObstaRace</div>
                    <div className="hidden md:block italic">Born in the mud</div>
                </div>

            </div>
        </div>
    );
}

export default Footer;