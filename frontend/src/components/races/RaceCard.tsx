import { type Race } from "../../Models/Races";
interface RaceCardProps {
    race: Race;
}
export function RaceCard({ race } : RaceCardProps) {
    return (
        <div className="group relative bg-[#1a2420] border border-white/5 overflow-hidden hover:border-[var(--color-accent)]/50 transition-all duration-500 shadow-2xl flex flex-col h-full">
            <div className="relative h-56 overflow-hidden">
                <div className="absolute inset-0 bg-black/20 z-10 group-hover:bg-transparent transition-all duration-500"></div>
                <img
                    src={race.imageUrl || "https://images.unsplash.com/photo-1594882645126-14020914d58d?q=80&w=800"}
                    alt={race.name}
                    className="w-full h-full object-cover grayscale group-hover:grayscale-0 group-hover:scale-110 transition-all duration-700"
                />
                <div className="absolute top-4 left-4 z-20 flex flex-col gap-2">
                    <div className="bg-[var(--color-accent)] text-[var(--color-dark)] text-[10px] font-black px-3 py-1 uppercase tracking-tighter shadow-xl">
                        {race.difficulty}
                    </div>
                    <div className="bg-black/80 backdrop-blur-md text-white text-[9px] font-bold px-3 py-1 uppercase tracking-widest border border-white/10">
                        {race.status}
                    </div>
                </div>
                <div className="absolute bottom-4 right-4 z-20 bg-[var(--color-dark)]/80 backdrop-blur-md px-3 py-1 border border-white/5 flex items-center gap-2">
                    <div className="w-1 h-3 bg-[var(--color-accent)]"></div>
                    <div className="text-[10px] font-black text-white uppercase tracking-widest">
                        +{race.elevationGain}m
                    </div>
                </div>
            </div>
            <div className="p-6 flex flex-col flex-grow">
                <div className="mb-6">
                    <h3 className="text-2xl font-black uppercase tracking-tighter leading-[1.1] group-hover:text-[var(--color-accent)] transition-colors min-h-[3.5rem] line-clamp-2">
                        {race.name}
                    </h3>
                    <div className="flex items-center gap-2 mt-2 opacity-50">
                        <span className="text-[10px] uppercase font-bold tracking-[0.2em]">{race.location}</span>
                    </div>
                </div>
                <div className="grid grid-cols-2 gap-4 mb-8">
                    <div className="flex flex-col">
                        <span className="text-[9px] uppercase tracking-[0.3em] opacity-40 mb-1">Distance</span>
                        <div className="flex items-baseline gap-1 font-black text-xl">
                            {race.distance} <span className="text-[10px] opacity-40">KM</span>
                        </div>
                    </div>
                    <div className="flex flex-col border-l border-white/10 pl-4">
                        <span className="text-[9px] uppercase tracking-[0.3em] opacity-40 mb-1">Start Date</span>
                        <div className="text-sm font-bold">
                            {new Date(race.date).toLocaleDateString('sr-RS')}
                        </div>
                    </div>
                </div>
                <div className="mt-auto pt-6 border-t border-white/5 flex items-center justify-between">
                    <div className="flex flex-col">
                        <span className="text-[8px] uppercase tracking-widest opacity-30">Deadline</span>
                        <span className="text-[10px] font-bold text-[var(--color-secondary)]">
                            {new Date(race.registrationDeadLine).toLocaleDateString('sr-RS')}
                        </span>
                    </div>

                    <div className="relative group/btn cursor-pointer">
                        <div className="absolute -inset-2 bg-[var(--color-accent)] opacity-0 group-hover/btn:opacity-10 blur-xl transition-all"></div>
                        <div className="relative bg-transparent border border-[var(--color-secondary)] text-[var(--color-secondary)] hover:bg-[var(--color-secondary)] hover:text-white px-6 py-2.5 text-[10px] font-black uppercase tracking-widest transition-all duration-300">
                            View Details
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
export default RaceCard;