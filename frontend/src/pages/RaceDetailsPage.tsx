import {Link, useParams} from "react-router";
import {useEffect, useState} from "react";
import raceService from "../services/raceService.ts";
import {Difficulty, type RaceDto, Status} from "../Models/races.type.ts";


export function RaceDetailsPage() {
    const {slug} = useParams<{slug:string}>();
    const [race, setRace] = useState<RaceDto|null>();
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const fetchRace = async () =>{
            if(slug){
                try{
                    const data = await raceService.raceDetails(slug);
                    setRace(data);
                }catch (e){
                    console.error(e);
                }finally {
                    setLoading(false);
                }
            }
        };
        void fetchRace();
    }, [slug]);
    if(loading)return <div className="min-h-screen bg-dark flex items-center justify-center text-accent font-black uppercase tracking-[0.5em] animate-pulse">Loading Arena...</div>
    if(!race)return <div className="min-h-screen bg-dark flex items-center justify-center text-secondary font-black uppercase italic">Mission not found.</div>
    const getDaysRemaining = (deadline:string)=>
    {
        const today = new Date();
        const target = new Date(deadline);
        const diffInMs = target.getTime() - today.getTime();
        const diffInDays = Math.ceil(diffInMs / (1000 * 60 * 60 * 24));
        if(diffInDays<0)return "Completed :(";
        if(diffInDays==0)return "Ongoing!";
        if(diffInDays==1)return "1 day left!";
        return `${diffInDays} days left`;
    }
    const isClosed = new Date(race.registrationDeadLine).getTime() < new Date().getTime();
    return (
        <div className="relative min-h-screen bg-dark text-light overflow-x-hidden selection:bg-accent selection:text-dark">
            <div
                className="absolute inset-0 z-0 pointer-events-none"
                style={{
                    backgroundImage: `url('${race.imageUrl}')`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                }}
            >
                <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]"></div>
                <div className="absolute inset-0 bg-linear-to-b from-dark via-transparent to-dark"></div>
                <div className="absolute inset-0 bg-linear-to-r from-dark via-dark/40 to-transparent"></div>
            </div>

            <div className="relative z-20 container mx-auto px-6 sm:px-12 lg:px-16 pt-32 pb-20">
                <div className="inline-flex items-center gap-2 text-accent text-xs font-black uppercase tracking-[0.3em] mb-12 hover:opacity-70 transition-all cursor-pointer">
                    <Link to={"/races"}><span>[</span> Back to Arena <span>]</span></Link>
                </div>
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-12 lg:gap-20">
                    <div className="lg:col-span-8">
                        <h1 className="text-white text-5xl md:text-8xl lg:text-9xl font-black uppercase tracking-tighter leading-[0.85] mb-8">
                            {race.name.split(' ')[0]} <br />
                            <span className="text-transparent bg-clip-text bg-linear-to-r from-accent to-secondary">
                            {race.name.split(' ').slice(1).join(' ')}.
                        </span>
                        </h1>
                        <div className="flex flex-wrap gap-4 mb-12">
                            <div className="bg-accent text-dark px-4 py-1 text-[10px] font-black uppercase italic tracking-tighter">
                                Difficulty: {Difficulty[race.difficulty]}
                            </div>
                            <div className="bg-white/10 backdrop-blur-md border border-white/20 px-4 py-1 text-[10px] font-black uppercase tracking-widest">
                                Status: {Status[race.status]}
                            </div>
                        </div>

                        <div className="max-w-3xl space-y-12">
                            <p className="text-light/90 text-lg md:text-2xl font-medium leading-relaxed border-l-4 border-secondary pl-6 italic">
                                {race.description || "No tactical briefing available for this sector."}
                            </p>
                            <div className="grid grid-cols-2 sm:grid-cols-3 gap-8 pt-10 border-t border-white/10">
                                <div className="group">
                                    <span className="block text-[10px] uppercase tracking-[0.4em] text-white/30 mb-2 group-hover:text-accent transition-colors">Distance</span>
                                    <p className="text-4xl font-black italic">{race.distance} <span className="text-sm opacity-30">KM</span></p>
                                </div>
                                <div className="group sm:border-l border-white/10 sm:pl-8">
                                    <span className="block text-[10px] uppercase tracking-[0.4em] text-white/30 mb-2 group-hover:text-accent transition-colors">Vertical Gain</span>
                                    <p className="text-4xl font-black italic">+{race.elevationGain} <span className="text-sm opacity-30">M</span></p>
                                </div>
                                <div className="group sm:border-l border-white/10 sm:pl-8">
                                    <span className="block text-[10px] uppercase tracking-[0.4em] text-white/30 mb-2 group-hover:text-accent transition-colors">Capacity</span>
                                    <p className="text-4xl font-black italic">{race.maxParticipants} <span className="text-sm opacity-30">MAX</span></p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="lg:col-span-4">
                        <div className="bg-dark/40 backdrop-blur-xl border border-white/10 p-8 md:p-10 shadow-2xl relative lg:sticky lg:top-32 overflow-hidden">
                            <div className="absolute top-0 right-0 w-24 h-24 bg-accent/5 -mr-12 -mt-12 rotate-45 pointer-events-none"></div>

                            <h3 className="text-2xl font-black uppercase tracking-tighter mb-10 italic border-b border-white/10 pb-4">
                                Deployment Info
                            </h3>
                            <div className="bg-secondary/15 border-l-2 border-secondary p-3 mb-6">
                                <div className="flex items-center gap-2 mb-1">
                                    <div className="w-1.5 h-1.5 rounded-full bg-secondary animate-pulse shadow-[0_0_5px_rgba(255,165,0,0.5)]"></div>
                                    <span className="text-secondary text-[10px] font-black uppercase tracking-[0.2em]">
                                        Time Remaining
                                    </span>
                                </div>
                                <p className="text-xl font-black italic text-white uppercase tracking-tight">
                                    {getDaysRemaining(race.registrationDeadLine)}
                                </p>
                            </div>
                            <div className="space-y-8 mb-12">
                                <div className="space-y-1">
                                    <span className="text-[10px] uppercase tracking-[0.3em] opacity-40 font-bold italic">Start Date</span>
                                    <p className="text-xl font-black italic uppercase">
                                        {new Date(race.date).toLocaleDateString('sr-RS', {
                                            day: '2-digit',
                                            month: '2-digit',
                                            year: 'numeric'
                                        })} // {new Date(race.date).toLocaleTimeString('sr-RS', { hour: '2-digit', minute: '2-digit' })}
                                    </p>
                                </div>
                                <div className="space-y-1">
                                    <span className="text-[10px] uppercase tracking-[0.3em] text-secondary font-bold italic">Registration Deadline</span>
                                     <p className="text-xl font-black italic text-secondary uppercase">
                                        {new Date(race.registrationDeadLine).toLocaleDateString('sr-RS', {
                                            day: '2-digit',
                                            month: '2-digit',
                                            year: 'numeric'
                                        })} // {new Date(race.registrationDeadLine).toLocaleTimeString('sr-RS', { hour: '2-digit', minute: '2-digit' })}
                                    </p>
                                </div>

                                <div className="space-y-1">
                                    <span className="text-[10px] uppercase tracking-[0.3em] opacity-40 font-bold italic">Location Sector</span>
                                    <p className="text-base font-bold italic leading-tight uppercase">{race.location}</p>
                                </div>
                            </div>
                            <button
                                disabled={isClosed}
                                className={`w-full group relative px-8 py-5 font-black uppercase tracking-tighter transition-all text-center text-sm italic
                                ${isClosed
                                    ? "bg-white/10 text-white/30 cursor-not-allowed border border-white/5"
                                    : "bg-accent text-dark hover:-translate-y-1 hover:shadow-[0_8px_0_0_rgba(166,124,82,1)] active:translate-y-0 active:shadow-none cursor-pointer"}
                                `}>
                                {isClosed ? "Registration Closed" : "Initialize Registration"}
                            </button>

                            <div className="mt-8 pt-8 border-t border-white/5 flex flex-col gap-2 opacity-20">
                                <p className="text-[8px] uppercase tracking-widest font-black italic">Protocol: Secure Entry</p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default RaceDetailsPage;