import RaceFilters from "../components/races/RaceFilters.tsx";
import RaceCard from "../components/races/RaceCard.tsx";
import {useCallback, useEffect, useState} from "react";
import raceService from "../services/raceService.ts";
import { useInfiniteScroll } from "../hooks/UseInfinityScroll.ts";
import type { RaceDto } from "../Models/races.type.ts";
import {useSearchParams} from "react-router";

type FiltersType = { search: string; difficulty: string; distance: string };

function RacesPage() {
    const [searchParams, setSearchParams] = useSearchParams();
    const [filters, setFilters] = useState<FiltersType>({
        search: searchParams.get("search") || "",
        difficulty: searchParams.get("difficulty") || "",
        distance: searchParams.get("distance") || ""
    });
    useEffect(() => {
        const params = new URLSearchParams();
        if (filters.search) {
            params.set("search", filters.search);
        }
        if (filters.difficulty && filters.difficulty !== "all") {
            params.set("difficulty", filters.difficulty);
        }
        if (filters.distance && filters.distance !== "Any distance") {
            params.set("distance", filters.distance);
        }
        setSearchParams(params, { replace: true });
    }, [filters, setSearchParams]);
    const fetchRaces = useCallback(
        (page: number) => raceService.races({
            search: filters.search,
            difficulty: filters.difficulty,
            distance: filters.distance,
            page
        }),
        [filters.search, filters.difficulty, filters.distance]
    );

    const { items: races, loading, hasMore, sentinelRef } = useInfiniteScroll<RaceDto>(
        fetchRaces,
        [filters.search, filters.difficulty, filters.distance]
    );

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

                    <RaceFilters filters={filters} setFilters={setFilters} />

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                        {loading ? (
                            <div className="col-span-full text-center py-20 opacity-50 uppercase tracking-[0.3em] text-[10px] font-black animate-pulse">
                                Loading arena...
                            </div>
                        ) : races.length > 0 ? (
                            races.map((race) => <RaceCard key={race.id} race={race} />)
                        ) : (
                            <div className="col-span-full text-center py-20 opacity-30 uppercase tracking-[0.3em] text-[10px] font-black border border-dashed border-white/5">
                                No races found matching your criteria
                            </div>
                        )}
                    </div>

                    <div ref={sentinelRef} className="h-10 mt-4" />

                    {!hasMore && races.length > 0 && (
                        <div className="text-center py-8 opacity-20 uppercase tracking-[0.3em] text-[10px] font-black border-t border-white/5">
                            — End of results —
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default RacesPage;