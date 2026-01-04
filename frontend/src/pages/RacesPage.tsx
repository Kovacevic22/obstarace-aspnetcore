/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react-hooks/exhaustive-deps */
import RaceFilters from "../components/races/RaceFilters.tsx";
import {useEffect, useState} from "react";
import type {RaceDto} from "../Models/races.type.ts";
import raceService from "../services/raceService.ts";
import {AxiosError} from "axios";
import RaceCard from "../components/races/RaceCard.tsx";
import {useSearchParams} from "react-router";
function RacesPage() {
    const [races, setRaces] = useState<RaceDto[]>([]);
    const [searchParams, setSearchParams] = useSearchParams();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    /////////////////////////////////////////////////////////
    const diffMap: any = { "0": "easy", "1": "medium", "2": "hard" };
    const revDiffMap: any = { "easy": "0", "medium": "1", "hard": "2" };

    const distMap: any = { "5": "short", "15": "mid", "1000": "long" };
    const revDistMap: any = { "short": "5", "mid": "15", "long": "1000" };
    ////////////////////////////////////////////////////////////
    const filters = {
        search: searchParams.get("search") || "",
        difficulty: revDiffMap[searchParams.get("difficulty") || ""] || searchParams.get("difficulty") || "all",
        distance: revDistMap[searchParams.get("distanceRange") || ""] || searchParams.get("distanceRange") || "all",
    }
    const updateFilters = (newFilters: any) => {
        const params: any = {};
        if (newFilters.search) params.search = newFilters.search;
        if (newFilters.difficulty !== "all") params.difficulty = diffMap[newFilters.difficulty];
        if (newFilters.distance !== "all" && newFilters.distance !== "Any distance")params.distanceRange = distMap[newFilters.distance];

        setSearchParams(params);
    };
    useEffect(()=>{
        const fetchRaces = async () =>{
            setLoading(true);
            try{
                const data = await raceService.races(filters);
                setRaces(data);
            }catch (err){
                if (err instanceof AxiosError) {
                    setError(err.response?.data?.error || "Error loading races.");
                } else {
                    setError("Error loading races.");
                }
            }finally {
                setLoading(false);
            }
        };
        const timeout = setTimeout(() => {
            void fetchRaces();
        }, 300);
        return () => clearTimeout(timeout);
    },[searchParams]);
    return (
        <>
            <div className="flex flex-col min-h-screen bg-dark">

                <div className="grow pt-32 pb-20 px-6 text-light">
                    <div className="max-w-7xl mx-auto">
                        <div className="mb-12">
                            <h1 className="text-4xl md:text-6xl font-black uppercase tracking-tighter mb-4">
                                Upcoming <span className="text-accent">Races</span>
                            </h1>
                            <div className="w-20 h-1.5 bg-secondary"></div>
                        </div>

                        <RaceFilters filters={filters} setFilters={updateFilters}/>

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
                                races.map((race) => (
                                    <RaceCard key={race.id} race={race} />
                                ))
                            ) : (
                                <div className="col-span-full text-center py-20 opacity-30 uppercase tracking-[0.3em] text-[10px] font-black border border-dashed border-white/5">
                                    No races found matching your criteria
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default RacesPage;