import { Role } from "../../Models/users.type.ts";
import { useState } from "react";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import { useAuth } from "../../hooks/useAuth.ts";
import { useProfileData } from "../../hooks/useProfileData.ts";

export function ProfilePage() {
    const { user } = useAuth();
    const {
        form, setForm, loading, error, setError, isFormValid, updateProfile
    } = useProfileData(user);

    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false);

    if (!user) return (
        <div className="min-h-screen bg-[#0a0c0b] flex items-center justify-center relative overflow-hidden">
            <div className="text-accent font-black uppercase tracking-[0.5em] animate-pulse italic">
                Authentication Required // Access Denied
            </div>
        </div>
    );

    const handleSave = async () => {
        const success = await updateProfile();
        if (success) {
            setIsConfirmModalOpen(false);
            window.location.reload();
        }
    };

    return (
        <div className="min-h-screen bg-[#0a0c0b] text-white selection:bg-accent selection:text-dark relative overflow-hidden">
            <div className="absolute inset-0 opacity-[0.03] pointer-events-none" style={{ backgroundImage: `linear-gradient(#fff 1px, transparent 1px), linear-gradient(90deg, #fff 1px, transparent 1px)`, backgroundSize: '40px 40px' }} />

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
                        </div>
                    </div>

                    <button
                        onClick={() => setIsConfirmModalOpen(true)}
                        disabled={loading || !isFormValid}
                        className={`cursor-pointer group relative bg-accent text-dark px-10 py-4 font-black uppercase italic text-xs tracking-widest overflow-hidden transition-all 
                        ${loading ? 'opacity-50' : 'hover:bg-white hover:-translate-y-1'}`}
                    >
                        <span className="relative z-10">{loading ? "Synchronizing..." : "Save_Changes"}</span>
                    </button>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-12 gap-16">
                    <div className="lg:col-span-8 space-y-16">
                        <section className="space-y-10">
                            <div className="flex items-center gap-4">
                                <h3 className="text-white/20 text-[11px] font-black uppercase tracking-[0.4em] italic">01 // Personal_Data</h3>
                                <div className="h-px flex-1 bg-gradient-to-r from-white/10 to-transparent"></div>
                            </div>

                            <div className="grid grid-cols-1 md:grid-cols-2 gap-x-10 gap-y-10">
                                <div className="flex flex-col gap-3">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic">Tactical_Email</label>
                                    <input type="text" value={user.email} disabled className="bg-white/2 border border-white/5 px-5 py-4 text-sm font-bold text-white/30 cursor-not-allowed italic" />
                                </div>

                                <div className="flex flex-col gap-3">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic">Emergency_Comms</label>
                                    <input
                                        value={form.emergencyContact}
                                        onChange={(e) => setForm({...form, emergencyContact: e.target.value})}
                                        type="text" placeholder="NAME / CONTACT" className="bg-white/3 border border-white/10 px-5 py-4 text-sm font-bold text-white outline-none focus:border-accent/50 transition-all" />
                                </div>

                                <div className="flex flex-col gap-3">
                                    <label className="text-[9px] uppercase tracking-widest text-white/30 font-black italic">Cycle_Of_Birth</label>
                                    <input
                                        value={form.dateOfBirth}
                                        onChange={(e) => setForm({...form, dateOfBirth: e.target.value})}
                                        type="date" className="bg-white/3 border border-white/10 px-5 py-4 text-sm font-bold text-white outline-none focus:border-accent/50 transition-all w-full" />
                                </div>
                            </div>
                        </section>
                    </div>
                    <div className="lg:col-span-4 lg:pl-10 border-l border-white/5">
                        <div className="bg-[#0f1110]/80 backdrop-blur-xl border border-white/5 p-10 space-y-12 sticky top-40 overflow-hidden group">
                            <div className="space-y-8">
                                <div className="flex justify-between items-end border-b border-white/5 pb-4">
                                    <span className="text-[9px] uppercase tracking-widest text-white/30 font-black">Total_Races</span>
                                    <span className="text-4xl font-black italic tracking-tighter text-white">{user.participant?.activity.totalRaces || 0}</span>
                                </div>
                            </div>
                            <p className="text-[8px] leading-relaxed uppercase font-bold tracking-[0.2em] text-white/10 italic pt-4">Automated_System_Tracking</p>
                        </div>
                    </div>
                </div>
            </div>

            <ConfirmModal
                isOpen={isConfirmModalOpen}
                onClose={() => { setIsConfirmModalOpen(false); setError(null); }}
                onConfirm={handleSave}
                title="SAVE_CHANGES"
                message="WARNING: ARE YOU SURE YOU WANT TO OVERWRITE YOUR DATA?"
                error={error}
                variant={"success"}
            />
        </div>
    );
}

export default ProfilePage;