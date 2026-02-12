import RaceFilters from "../components/races/RaceFilters.tsx";
import RaceCard from "../components/races/RaceCard.tsx";
import { useRaceFilters } from "../hooks/useRaceFilters.ts";

function RacesPage() {
    const { races, filters, updateFilters, loading, error } = useRaceFilters();

    return (
        <div className="flex flex-col min-h-screen bg-dark">
            <div className="grow pt-32 pb-20 px-6 text-light">
                <div className="max-w-7xl mx-auto">
                    <div className="mb-12">
                        <h1 className="text-4xl md:text-6xl font-black uppercase tracking-tighter mb-4">
                            Upcoming <span className="text-accent">Races</span>
                        </h1>
                        <div className="w-20 h-1.5 bg-secondary"></div>
                    </div>

                    <RaceFilters filters={filters} setFilters={updateFilters} />

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                        {loading ? (
                            <div className="col-span-full text-center py-20 opacity-50 uppercase tracking-[0.3em] text-[10px] font-black animate-pulse">
                                Loading arena...
                            </div>
                        ) : error ? (
                            <div className="col-span-full text-center py-20 text-red-500 uppercase tracking-[0.3em] text-[10px] font-black border border-red-500/20 bg-red-500/5">
                                {error}
                            </div>
                        ) : races.length > 0 ? (
                            races.map((race) => <RaceCard key={race.id} race={race} />)
                        ) : (
                            <div className="col-span-full text-center py-20 opacity-30 uppercase tracking-[0.3em] text-[10px] font-black border border-dashed border-white/5">
                                No races found matching your criteria
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default RacesPage;