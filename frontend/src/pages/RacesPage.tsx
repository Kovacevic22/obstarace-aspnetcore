import RaceFilters from "../components/races/RaceFilters.tsx";
function RacesPage() {
    return (
        <>
            <div className="flex flex-col min-h-screen bg-[var(--color-dark)]">

                <div className="flex-grow pt-32 pb-20 px-6 text-[var(--color-light)]">
                    <div className="max-w-7xl mx-auto">
                        <div className="mb-12">
                            <h1 className="text-4xl md:text-6xl font-black uppercase tracking-tighter mb-4">
                                Upcoming <span className="text-[var(--color-accent)]">Races</span>
                            </h1>
                            <div className="w-20 h-1.5 bg-[var(--color-secondary)]"></div>
                        </div>

                        <RaceFilters />

                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                            <div className="col-span-full text-center py-20 opacity-30 uppercase tracking-[0.3em] text-[10px] font-black border border-dashed border-white/5">
                                No races found matching your criteria
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default RacesPage;