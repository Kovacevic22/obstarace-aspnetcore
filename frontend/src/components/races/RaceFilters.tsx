interface RaceFiltersProps {
    filters: { search: string; difficulty: string; distance: string };
    setFilters: (filters: any) => void;
}

export function RaceFilters({ filters, setFilters }: RaceFiltersProps) {
    const handleReset = () => {
        setFilters({ search: "", difficulty: "all", distance: "Any distance" });
    };
    return (
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-16 items-end">
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Search</label>
                <input
                    type="text"
                    placeholder="Race name..."
                    value={filters.search}
                    onChange={(e) => setFilters({...filters, search: e.target.value })}
                    className="bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-accent transition-colors text-white"
                />
            </div>
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Difficulty</label>
                <div className="relative">
                    <select
                        value={filters.difficulty}
                        onChange={(e) => setFilters({...filters, difficulty: e.target.value })}
                        className="w-full bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-accent appearance-none cursor-pointer text-white">
                        <option value={"all"} className="bg-dark">All Levels</option>
                        <option value={0} className="bg-dark">Easy</option>
                        <option value={1} className="bg-dark">Medium</option>
                        <option value={2} className="bg-dark">Hard</option>
                    </select>
                    <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-50">
                        <div
                            className="border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-white"></div>
                    </div>
                </div>
            </div>
            <div className="flex flex-col gap-2">
                <label className="text-[10px] font-black uppercase tracking-widest opacity-50 ml-1">Distance
                    (km)</label>
                <div className="relative">
                    <select
                        value={filters.distance}
                        onChange={(e) => setFilters({...filters, distance: e.target.value })}
                        className="w-full bg-white/5 border border-white/10 px-4 py-3 text-sm focus:outline-none focus:border-accent appearance-none cursor-pointer text-white">
                        <option value={"Any distance"} className="bg-dark">Any Distance</option>
                        <option value="5" className="bg-dark">0 - 5 km</option>
                        <option value="15" className="bg-dark">5 - 15 km</option>
                        <option value="1000" className="bg-dark">15+ km</option>
                    </select>
                    <div className="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none opacity-50">
                        <div
                            className="border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-white"></div>
                    </div>
                </div>
            </div>
            <button
                onClick={handleReset}
                className="h-11.5 bg-white/5 border border-white/20 text-[10px] font-black uppercase tracking-widest hover:bg-accent hover:text-dark transition-all cursor-pointer">
                Reset Filters
            </button>
        </div>
    );
}

export default RaceFilters;