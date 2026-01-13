import {Role, type UserDto} from "../../Models/users.type.ts";
import {useState} from "react";
import userService from "../../services/userService.ts";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import type {UpdateParticipantDto} from "../../Models/participant.type.ts";

interface Props {
    user: UserDto | null;
}

export function ProfilePage({ user }: Props) {
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false);
    const [formUpdated, setFormUpdated] = useState<UpdateParticipantDto>({
        dateOfBirth: user?.participant?.dateOfBirth
            ? new Date(user.participant.dateOfBirth).toISOString().split('T')[0]
            : "",
        emergencyContact: user?.participant?.emergencyContact || "",
    });
    if (!user) return (
        <div className="min-h-screen bg-[#0a0c0b] flex items-center justify-center relative overflow-hidden">
            <div className="absolute inset-0 bg-[radial-gradient(circle_at_center,var(--tw-gradient-stops))] from-accent/5 via-transparent to-transparent opacity-50" />
            <div className="text-accent font-black uppercase tracking-[0.5em] animate-pulse italic relative z-10">
                Authentication Required // Access Denied
            </div>
        </div>
    );
    const parseApiError = (error: any): string => {
        const data = error?.response?.data;

        if (data?.errors) {
            return Object.values(data.errors).flat().join(" | ");
        }

        return data?.title || data?.error || "Error";
    };
    const isFormValid = formUpdated.emergencyContact!="" && formUpdated.emergencyContact!.length > 0;
    const updateUser = async ()=>{
        try{
            setLoading(true);
            setError(null);
            await userService.updateUser(user.id,formUpdated);
            setIsConfirmModalOpen(false);
            window.location.reload();
        }catch (err){
            console.error(err);
            setError(parseApiError(err));
        }finally{
            setLoading(false);
        }
    }
    return (
        <div className="min-h-screen bg-[#0a0c0b] text-white selection:bg-accent selection:text-dark relative overflow-hidden">
            <div className="absolute inset-0 opacity-[0.03] pointer-events-none"
                 style={{ backgroundImage: `linear-gradient(#fff 1px, transparent 1px), linear-gradient(90deg, #fff 1px, transparent 1px)`, backgroundSize: '40px 40px' }} />
            <div className="absolute -top-[10%] -left-[10%] w-[40%] h-[40%] bg-accent/5 rounded-full blur-[120px] pointer-events-none" />
            <div className="absolute top-[20%] -right-[5%] w-[30%] h-[50%] bg-white/5 rounded-full blur-[100px] rotate-45 pointer-events-none" />


            <div className="absolute inset-0 bg-[radial-gradient(circle_at_center,transparent_0%,#0a0c0b_100%)] pointer-events-none" />


            <div className="max-w-6xl mx-auto pt-40 pb-24 px-6 space-y-24 relative z-10">

                <div className="flex flex-col md:flex-row justify-between items-start md:items-end border-b border-white/5 pb-12 gap-8">
                    <div className="space-y-3">
                        <div className="flex items-center gap-2">
                            <div className="w-2 h-2 bg-accent animate-pulse" />
                            <p className="text-accent text-[10px] font-black uppercase tracking-[0.5em]">System_Profile // ID: {user.id}</p>
                        </div>
                        <h1 className="text-6xl md:text-8xl font-black uppercase tracking-[-0.05em] leading-[0.8]">
                            {user.participant?.name}<br/>
                            <span className="text-white/10">{user.participant?.surname}</span>
                        </h1>
                        <div className="flex gap-4 mt-6">
                            <div className="px-3 py-1 bg-white/5 border border-white/10 text-[9px] font-black uppercase tracking-widest text-white/40 backdrop-blur-sm">
                                Unit_Type: <span className="text-white">{Role[user.role]}</span>
                            </div>
                            <div className="px-3 py-1 bg-accent/10 border border-accent/20 text-[9px] font-black uppercase tracking-widest text-accent italic backdrop-blur-sm">
                                Status: Verified_Athlete
                            </div>
                        </div>
                    </div>

                    <button
                        onClick={()=>setIsConfirmModalOpen(true)}
                        disabled={loading || !isFormValid}
                        className={`cursor-pointer group relative bg-accent text-dark px-10 py-4 font-black uppercase italic text-xs tracking-widest overflow-hidden transition-all 
                        ${loading ? 'opacity-50 cursor-wait' : 'hover:bg-white hover:-translate-y-1'}`}
                    >
                        <span className="relative z-10">{loading ? "Synchronizing..." : "Save_Changes"}</span>
                        <div className="absolute inset-0 bg-white/20 -translate-x-full group-hover:translate-x-full transition-transform duration-500 skew-x-12" />
                    </button>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-12 gap-16">
                    <div className="lg:col-span-8 space-y-16">
                        <section className="space-y-10">
                            <div className="flex items-center gap-4">
                                <h3 className="text-white/20 text-[11px] font-black uppercase tracking-[0.4em] italic">01 // Personal_Data</h3>
                                <div className="h-px flex-1 bg-linear-to-r from-white/10 to-transparent"></div>
                            </div>

                            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-10 gap-y-10">
                                <div className="flex flex-col gap-3 group">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic group-focus-within:text-accent transition-colors">Tactical_Email</label>
                                    <input type="text" defaultValue={user.email} disabled className="bg-white/2 border border-white/5 px-5 py-4 text-sm font-bold text-white/30 outline-none border-dashed cursor-not-allowed italic transition-all" />
                                </div>

                                <div className="flex flex-col gap-3 group">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic group-focus-within:text-accent transition-colors">Emergency_Comms</label>
                                    <input
                                        value={formUpdated.emergencyContact}
                                        onChange={(e)=>setFormUpdated({...formUpdated,emergencyContact:e.target.value})}
                                        type="text" defaultValue={user.participant?.emergencyContact} placeholder="NAME / CONTACT" className="bg-white/3 border border-white/10 px-5 py-4 text-sm font-bold text-white outline-none focus:border-accent/50 focus:bg-white/5 transition-all" />
                                </div>

                                <div className="flex flex-col gap-3 group">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic group-focus-within:text-accent transition-colors">Contact_Frequency</label>
                                    <input
                                        disabled={true}
                                        readOnly={true}
                                        type="text" defaultValue={user.phoneNumber} className="bg-white/2 border border-white/5 px-5 py-4 text-sm font-bold text-white/30 outline-none border-dashed cursor-not-allowed italic transition-all" />
                                </div>

                                <div className="flex flex-col gap-3 group">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic group-focus-within:text-accent transition-colors">Cycle_Of_Birth</label>
                                    <input
                                        value={formUpdated.dateOfBirth}
                                        onChange={(e)=>setFormUpdated({...formUpdated,dateOfBirth:e.target.value})}
                                        type="date"
                                        defaultValue={user.participant?.dateOfBirth ? new Date(user.participant?.dateOfBirth).toISOString().split('T')[0] : ""}
                                        className="bg-white/3 border border-white/10 px-5 py-4 text-sm font-bold text-white outline-none focus:border-accent/50 focus:bg-white/5 transition-all w-full"
                                    />
                                </div>
                            </div>
                        </section>

                        <section className="space-y-6 pt-4">
                            <div className="flex items-center gap-4">
                                <h3 className="text-white/20 text-[11px] font-black uppercase tracking-[0.4em] italic">02 // Security</h3>
                                <div className="h-px flex-1 bg-linear-to-r from-white/10 to-transparent"></div>
                            </div>
                            <button className="text-[10px] font-black uppercase italic text-accent hover:text-white transition-colors cursor-pointer border-b border-accent/20 pb-1">
                                [ Password_Change ]
                            </button>
                        </section>
                    </div>

                    <div className="lg:col-span-4 lg:pl-10 border-l border-white/5">
                        <div className="bg-[#0f1110]/80 backdrop-blur-xl border border-white/5 p-10 space-y-12 sticky top-40 overflow-hidden group">
                            <div className="absolute top-0 left-0 w-0.5 h-0 bg-accent group-hover:h-full transition-all duration-700" />

                            <div className="space-y-2">
                                <h4 className="text-[10px] font-black uppercase tracking-widest text-accent italic">Activity_Log</h4>
                                <div className="h-0.5 w-8 bg-accent"></div>
                            </div>

                            <div className="space-y-8">
                                <div className="flex justify-between items-end border-b border-white/5 pb-4">
                                    <span className="text-[9px] uppercase tracking-widest text-white/30 font-black">Total_Races</span>
                                    <span className="text-4xl font-black italic tracking-tighter text-white">{user.participant?.activity.totalRaces || 0}</span>
                                </div>

                                <div className="flex justify-between items-end border-b border-white/5 pb-4">
                                    <span className="text-[9px] uppercase tracking-widest text-white/30 font-black">Finished_Races</span>
                                    <span className="text-4xl font-black italic tracking-tighter text-white">{user.participant?.activity.finishedRaces || 0}</span>
                                </div>
                            </div>

                            <p className="text-[8px] leading-relaxed uppercase font-bold tracking-[0.2em] text-white/10 italic pt-4">
                                Automated_System_Tracking // Data synchronized with Central Command.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <ConfirmModal
                isOpen={isConfirmModalOpen}
                onClose={() => setIsConfirmModalOpen(false)}
                onConfirm={updateUser}
                title="SAVE_CHANGES"
                message="WARNING: ARE YOU SURE YOU WANT TO OVERWRITE YOUR DATA?"
                error={error}
                variant={"success"}
            />
        </div>
    );
}

export default ProfilePage;