import { type RegistrationDto, RegistrationStatus } from "../../Models/registrations.type.ts";
import { useEffect, useState } from "react";
import { registrationService } from "../../services/registrationService.ts";
import type { UserDto } from "../../Models/users.type.ts";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import {Link} from "react-router";

interface Props {
    user: UserDto | null;
}

const MyRegistrations = ({ user }: Props) => {
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>("");
    const [registrations, setRegistrations] = useState<RegistrationDto[]>([]);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState<boolean>(false);
    const [itemToDelete, setItemToDelete] = useState<number | null>(null);

    const parseApiError = (error: any): string => {
        const data = error?.response?.data;
        if (data?.errors) return Object.values(data.errors).flat().join(" | ");
        return data?.title || data?.error || "Error";
    };

    const deleteRegistration = async () => {
        if (itemToDelete) {
            try {
                await registrationService.deleteRegistration(itemToDelete);
                setRegistrations(prev => prev.filter(r => r.id !== itemToDelete));
                setItemToDelete(null);
                setIsDeleteModalOpen(false);
                setError(null);
            } catch (err) {
                console.error(err);
                setError(parseApiError(err));
            }
        }
    };

    useEffect(() => {
        const fetchData = async () => {
            if (!user?.id) return;
            try {
                setLoading(true);
                const data = await registrationService.registrations(user.id);
                setRegistrations(data);
            } catch (err) {
                console.error(err);
                setError(parseApiError(err));
            } finally {
                setLoading(false);
            }
        };
        void fetchData();
    }, [user]);

    return (
        <div className="min-h-screen bg-[#0a0f0e] relative overflow-hidden text-white font-black italic uppercase px-6 md:px-12 pt-32 pb-20">
            <div
                className="absolute inset-0 opacity-[0.05] pointer-events-none"
                style={{ backgroundImage: `radial-gradient(circle, #ffffff 1px, transparent 1px)`, backgroundSize: '40px 40px' }}
            />
            <div className="absolute -top-24 -left-24 w-150 h-150 bg-accent/5 rounded-full blur-[150px] pointer-events-none animate-pulse" />

            <div className="relative z-10 max-w-6xl mx-auto">
                <div className="mb-16 border-l-4 border-accent pl-6 mt-4">
                    <h1 className="text-5xl md:text-8xl tracking-tighter leading-none select-none uppercase">
                        Registrations <span className="text-accent">Archive</span>
                    </h1>
                    <p className="text-[10px] tracking-[0.5em] mt-3 not-italic font-medium text-white/30 uppercase">
                        USER_LOG // {loading ? "SCANNING..." : `${registrations.length} ACTIVE_RECORDS_FOUND`}
                    </p>
                </div>

                <div className="space-y-6">
                    <div className="hidden md:grid grid-cols-5 px-8 py-4 text-[11px] tracking-[0.3em] text-white/20 border-b border-white/5 uppercase">
                        <div className="col-span-2">Operation / Location</div>
                        <div>Identification (BIB)</div>
                        <div>Status Report</div>
                        <div className="text-right">System Action</div>
                    </div>

                    {loading ? (
                        <div className="flex flex-col items-center justify-center py-32 bg-white/2 border border-white/5 border-dashed rounded-sm">
                            <div className="relative">
                                <div className="absolute inset-0 rounded-full bg-accent opacity-20 animate-ping"></div>
                                <div className="w-14 h-14 border-2 border-accent/10 border-t-accent rounded-full animate-spin"></div>
                            </div>
                            <div className="mt-8 flex flex-col items-center gap-2">
                                <div className="text-white/40 font-black uppercase tracking-[0.4em] text-[10px] animate-pulse">
                                    Synchronizing <span className="text-accent">Archive_Data</span>
                                </div>
                                <div className="w-24 h-px bg-white/5 relative overflow-hidden">
                                    <div className="absolute inset-0 bg-accent w-1/2 animate-[loading-bar_1.5s_infinite_ease-in-out]"></div>
                                </div>
                            </div>
                        </div>
                    ) : registrations.length > 0 ? (
                        registrations.map((reg) => (
                            <div
                                key={reg.id}
                                className="grid grid-cols-1 md:grid-cols-5 items-center bg-white/2 border border-white/5 p-8 gap-6 hover:bg-white/5 hover:border-accent/40 transition-all group relative overflow-hidden shadow-2xl backdrop-blur-sm"
                            >
                                <div className="md:col-span-2 space-y-2">
                                    <div className="flex items-center gap-3">
                                        <span className="text-[10px] font-mono text-accent bg-accent/10 px-2 py-0.5 border border-accent/20 italic">RACE_ID: #{reg.raceId}</span>
                                        <span className="text-[9px] font-mono text-white/10 uppercase tracking-tighter italic">REG_#{reg.id}</span>
                                    </div>
                                    <h3 className="text-2xl md:text-3xl tracking-tighter group-hover:text-accent transition-colors leading-tight uppercase font-black italic">
                                        {reg.race?.name}
                                    </h3>
                                    <p className="text-[10px] text-white/30 not-italic font-bold tracking-widest uppercase">
                                        Sector: {reg.race?.location}
                                    </p>
                                </div>
                                <div className="flex flex-col">
                                    <span className="text-[9px] text-white/20 md:hidden mb-2 font-bold uppercase tracking-widest italic">Bib Number</span>
                                    <span className="text-4xl font-black tracking-widest text-white group-hover:scale-105 transition-transform origin-left">
                                        #{reg.bibNumber}
                                    </span>
                                </div>
                                <div className="flex flex-col">
                                    <span className="text-[9px] text-white/20 md:hidden mb-3 font-bold uppercase tracking-widest italic">Status Report</span>
                                    <div className={`inline-flex items-center gap-3 px-4 py-1.5 border w-fit ${
                                        reg.status === RegistrationStatus.Confirmed ? 'border-green-500/50 text-green-500 bg-green-500/5' :
                                            reg.status === RegistrationStatus.Pending ? 'border-orange-500/50 text-orange-500 bg-orange-500/5' :
                                                'border-red-500/50 text-red-500 bg-red-500/5'
                                    }`}>
                                        <span className={`w-2 h-2 rounded-full shadow-[0_0_8px_currentColor] ${
                                            reg.status === RegistrationStatus.Confirmed ? 'bg-green-500' :
                                                reg.status === RegistrationStatus.Pending ? 'bg-orange-500 animate-pulse' :
                                                    'bg-red-500'
                                        }`}></span>
                                        <span className="text-[11px] font-black tracking-[0.2em] uppercase italic">{RegistrationStatus[reg.status]}</span>
                                    </div>
                                </div>
                                <div className="text-right">
                                    {reg.status === RegistrationStatus.Pending ? (
                                        <button
                                            onClick={() => { setItemToDelete(reg.id); setIsDeleteModalOpen(true); }}
                                            className="text-[11px] text-white/20 hover:text-red-500 transition-all cursor-pointer font-black border-b border-transparent hover:border-red-500 pb-1 uppercase italic tracking-tighter"
                                        >
                                            [ DELETE_REG ]
                                        </button>
                                    ) : (
                                        <span className="text-[10px] text-white/5 italic uppercase tracking-widest select-none">[ RECORD_LOCKED ]</span>
                                    )}
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="mt-10 border-2 border-dashed border-white/5 p-24 text-center bg-white/1">
                            <p className="text-white/20 text-sm tracking-[0.8em] uppercase italic font-black">NO ACTIVE REGISTRATIONS</p>
                            <Link to={"/races"}>
                                <button className="cursor-pointer mt-10 px-12 py-5 bg-accent text-dark font-black hover:bg-white transition-all transform active:scale-95 shadow-[0_0_20px_rgba(166,124,82,0.2)] uppercase italic">
                                    _FIND_RACES_
                                </button>
                            </Link>
                        </div>
                    )}
                </div>

                <ConfirmModal
                    isOpen={isDeleteModalOpen}
                    onClose={() => setIsDeleteModalOpen(false)}
                    onConfirm={deleteRegistration}
                    error={error}
                    title="TERMINATE ARCHIVE"
                    message="ARE YOU SURE? YOU ARE ABOUT TO PERMANENTLY DELETE THIS REGISTRATION RECORD FROM THE SYSTEM. THIS ACTION CANNOT BE REVERTED."
                />
            </div>
        </div>
    );
};

export default MyRegistrations;