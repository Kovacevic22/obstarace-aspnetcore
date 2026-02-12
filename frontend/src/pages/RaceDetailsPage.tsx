import {Link, useParams} from "react-router";
import {useState} from "react";
import {Difficulty, Status} from "../Models/races.type.ts";
import ConfirmModal from "../components/common/ConfirmModal.tsx";
import CountRegistrations from "../components/common/CountRegistrations.tsx";
import {useRaceDetails} from "../hooks/useRaceDetails.ts";
import {useAuth} from "../hooks/useAuth.ts";

export function RaceDetailsPage() {
    const {user} = useAuth();
    const {slug} = useParams<{slug:string}>();
    const {
        race,
        loading,
        error,
        setError,
        isRegistering,
        isClosed,
        registerToRace,
        getDaysRemaining,
        isAlreadyRegistered
    } = useRaceDetails(slug, user?.id);

    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);

    if(loading) return <div className="min-h-screen bg-dark flex items-center justify-center text-accent font-black uppercase tracking-[0.5em] animate-pulse">Loading Arena...</div>
    if(!race) return <div className="min-h-screen bg-dark flex items-center justify-center text-secondary font-black uppercase italic">Mission not found.</div>

    const handleRegistration = async () => {
        const success = await registerToRace();
        if (success) {
            setIsConfirmModalOpen(false);
        }
    };

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
                                    <p className="text-4xl font-black italic"><CountRegistrations raceId={race.id}/>/{race.maxParticipants} <span className="text-sm opacity-30">MAX</span></p>
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
                                disabled={isClosed || isRegistering || isAlreadyRegistered}
                                onClick={() => setIsConfirmModalOpen(true)}
                                className={`w-full group relative px-8 py-5 font-black uppercase tracking-tighter transition-all text-center text-sm italic
                                ${(isClosed || isAlreadyRegistered)
                                    ? "bg-white/10 text-white/30 cursor-not-allowed border border-white/5"
                                    : "bg-accent text-dark hover:-translate-y-1 hover:shadow-[0_8px_0_0_rgba(166,124,82,1)] active:translate-y-0 active:shadow-none cursor-pointer"}
                                `}
                            >
                                {isRegistering
                                    ? "SYCHRONIZING..."
                                    : isAlreadyRegistered
                                        ? "ALREADY_REGISTERED_ON_THIS_RACE"
                                        : isClosed
                                            ? "Registration Closed"
                                            : "Initialize Registration"}
                            </button>
                            <div className="mt-8 pt-8 border-t border-white/5 flex flex-col gap-2 opacity-20">
                                <p className="text-[8px] uppercase tracking-widest font-black italic">Protocol: Secure Entry</p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div className="mt-12 mb-20 border-t border-white/5 pt-8 relative z-30 max-w-5xl mx-auto w-full">
                <div className="flex flex-col gap-2 mb-8 px-4">
                    <h2 className="text-2xl md:text-3xl font-black italic uppercase tracking-tight text-white leading-none">
                        Obstacle <span className="text-accent">List</span>
                    </h2>
                    <p className="text-[11px] tracking-[0.5em] text-white/30 uppercase italic font-black">
                        Sector_Recon // {race.obstacles?.length || 0} Identified_Obstacles
                    </p>
                </div>
                {race.obstacles && race.obstacles.length > 0 ? (
                    <div className="overflow-hidden bg-white/2 border border-white/5 shadow-2xl backdrop-blur-sm rounded-sm mx-4">
                        <table className="w-full text-left border-collapse">
                            <thead>
                            <tr className="border-b border-white/10 bg-white/5 text-[10px] uppercase tracking-widest text-accent font-black">
                                <th className="py-4 px-6 italic">Obstacle name</th>
                                <th className="py-4 px-6 italic font-black">Description</th>
                                <th className="py-4 px-6 italic font-black text-center">Difficulty</th>
                            </tr>
                            </thead>
                            <tbody className="divide-y divide-white/5 italic font-black">
                            {race.obstacles.map((obstacle) => (
                                <tr key={obstacle.id} className="group hover:bg-white/4 transition-all">
                                    <td className="py-5 px-6">
                                        <span className="text-[9px] font-mono text-white/10 mb-1 block uppercase tracking-tighter not-italic font-bold">#0{obstacle.id}</span>
                                        <p className="text-lg uppercase text-white group-hover:text-accent transition-colors tracking-tighter">
                                            {obstacle.name}
                                        </p>
                                    </td>
                                    <td className="py-5 px-6">
                                        <p className="text-xs text-white/50 max-w-sm not-italic font-bold leading-relaxed lowercase first-letter:uppercase">
                                            {obstacle.description}
                                        </p>
                                    </td>
                                    <td className="py-5 px-6 text-center">
                                        <div className={`inline-block px-4 py-1 text-[9px] tracking-widest border font-black ${
                                            obstacle.difficulty === 2 ? 'border-red-500/40 text-red-500 bg-red-500/5' :
                                                obstacle.difficulty === 1 ? 'border-orange-500/40 text-orange-500 bg-orange-500/5' :
                                                    'border-green-500/40 text-green-500 bg-green-500/5'
                                        }`}>
                                            {Difficulty[obstacle.difficulty]}
                                        </div>
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                ) : (
                    <div className="py-12 text-center border border-dashed border-white/5 bg-white/1 mx-4">
                        <p className="text-white/10 text-[9px] tracking-[0.5em] font-black uppercase italic">
                            NO_OBSTACLES_DETECTED
                        </p>
                    </div>
                )}
            </div>
            <ConfirmModal
                isOpen={isConfirmModalOpen}
                onClose={() => { setIsConfirmModalOpen(false); setError(null); }}
                onConfirm={handleRegistration}
                title="CONFIRM REGISTRATION"
                message={`ARE YOU SURE YOU WANT TO REGISTER FOR ${race.name.toUpperCase()}? THIS ACTION WILL ASSIGN YOU A TACTICAL BIB NUMBER.`}
                error={error}
                variant="success"
            />
        </div>
    );
}

export default RaceDetailsPage;