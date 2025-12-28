export function RaceFilters() {
    return (
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-16 items-end">
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Search</label>
                <input
                    type="text"
                    placeholder="Race name..."
                    className="bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-[var(--color-accent)] transition-colors text-white"
                />
            </div>
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Difficulty</label>
                <div className="relative">
                    <select className="w-full bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-[var(--color-accent)] appearance-none cursor-pointer text-white">
                        <option className="bg-[var(--color-dark)]">All Levels</option>
                        <option className="bg-[var(--color-dark)]">Easy</option>
                        <option className="bg-[var(--color-dark)]">Medium</option>
                        <option className="bg-[var(--color-dark)]">Hard</option>
                    </select>
                    <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-50">
                        <div className="border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-white"></div>
                    </div>
                </div>
            </div>
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Distance (km)</label>
                <div className="relative">
                    <select className="w-full bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-[var(--color-accent)] appearance-none cursor-pointer text-white">
                        <option className="bg-[var(--color-dark)]">Any Distance</option>
                        <option className="bg-[var(--color-dark)]">0 - 5 km</option>
                        <option className="bg-[var(--color-dark)]">5 - 15 km</option>
                        <option className="bg-[var(--color-dark)]">15+ km</option>
                    </select>
                    <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-50">
                        <div className="border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-white"></div>
                    </div>
                </div>
            </div>
            <button className="h-[46px] bg-white/5 border border-white/20 text-[10px] font-black uppercase tracking-widest hover:bg-[var(--color-accent)] hover:text-[var(--color-dark)] transition-all cursor-pointer">
                Reset Filters
            </button>
        </div>
    );
}
export default RaceFilters;